using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Extensions
{
    /// <summary>
    /// The Dapper.Contrib extensions for Dapper
    /// </summary>
    public static partial class SqlMapperExtensions
    {
        /// <summary>
        /// Returns a single entity by a single id from table "Ts" asynchronously using Task. T must be of interface type. 
        /// Id must be marked with [Key] attribute.
        /// Created entity is tracked/intercepted for changes and used by the Update() extension. 
        /// </summary>
        /// <typeparam name="T">Interface type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="ids">Id of the entity to get, must be marked with [Key] attribute</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T</returns>
        public static async Task<T> GetAsync<T>(this IDbConnection connection, dynamic[] ids, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return (T)await GetAsync(connection, typeof(T), ids, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// Returns a single entity by a single id from table "Ts" asynchronously using Task. T must be of interface type. 
        /// Id must be marked with [Key] attribute.
        /// Created entity is tracked/intercepted for changes and used by the Update() extension. 
        /// </summary>
        /// <param name="type">Interface type to create and populate</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="ids">Id of the entity to get, must be marked with [Key] attribute</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T</returns>
        public static async Task<object> GetAsync(this IDbConnection connection, Type type, dynamic[] ids, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var keys = GetKeys(type, nameof(GetAsync));
            if (!_getQueries.TryGetValue(type.TypeHandle, out var sql))
            {
                var name = tableName ?? GetTableName(type);
                sql = $"SELECT * FROM `{name}` WHERE {keys.Select(k => $" `{k.Name}` = @{k.Name} ").ExpandToString("AND")}";
                _getQueries[type.TypeHandle] = sql;
            }

            var dynParams = ids.Zip(keys, (value, key) => new { value, key }).Aggregate(new DynamicParameters(), (p, k) => p.Add($"@{k.key.Name}", k.value));
            if (!type.IsInterface)
                return (await connection.QueryAsync(type, sql, dynParams, transaction, commandTimeout).ConfigureAwait(false)).FirstOrDefault();

            if (!((await connection.QueryAsync<dynamic>(sql, dynParams).ConfigureAwait(false)).FirstOrDefault() is IDictionary<string, object> res))
            {
                return null;
            }

            var obj = ProxyGenerator.GetInterfaceProxy(type);

            foreach (var property in TypePropertiesCache(type))
            {
                var val = res[property.Name];
                if (val == null) continue;
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var genericType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (genericType != null) property.SetValue(obj, Convert.ChangeType(val, genericType), null);
                }
                else
                {
                    property.SetValue(obj, Convert.ChangeType(val, property.PropertyType), null);
                }
            }
            ((IProxy)obj).IsDirty = false;   //reset change tracking and return

            return obj;
        }

        /// <summary>
        /// Returns a list of entities from table "Ts".  
        /// Id of T must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T</returns>
        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IDbConnection connection, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return (await GetAllAsync(connection, typeof(T),tableName, transaction, commandTimeout)).OfType<T>();
        }

        /// <summary>
        /// Returns a list of entities from table "Ts".  
        /// Id of T must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <param name="type">Interface or type to create and populate</param>
        /// <param name="tableName"></param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T</returns>
        public static Task<IEnumerable<object>> GetAllAsync(this IDbConnection connection, Type type, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cacheType = typeof(List<>).MakeGenericType(type);

            if (!_getQueries.TryGetValue(cacheType.TypeHandle, out var sql))
            {
                GetKeys(type, nameof(GetAllAsync));
                var name =tableName?? GetTableName(type);

                sql = "SELECT * FROM " + name;
                _getQueries[cacheType.TypeHandle] = sql;
            }

            if (!type.IsInterface)
            {
                return connection.QueryAsync(type, sql, null, transaction, commandTimeout);
            }
            return GetAllAsyncImpl(connection, transaction, commandTimeout, sql, type);
        }


        /// <summary>
        /// Inserts an entity into table "Ts" asynchronously using Task and returns identity id.
        /// </summary>
        /// <typeparam name="T">The type being inserted.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Identity of inserted entity</returns>
        public static Task<int> InsertAsync<T>(this IDbConnection connection, T entityToInsert, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return InsertAsync(connection, typeof(T), entityToInsert, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// Inserts an entity into table "Ts" asynchronously using Task and returns identity id.
        /// </summary>
        /// <param name="type">The type being inserted.</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Identity of inserted entity</returns>
        public static async Task<int> InsertAsync(this IDbConnection connection, Type type, object entityToInsert, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {

            var isList = false;
            if (type.IsArray)
            {
                isList = true;
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                var implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    isList = true;
                    type = type.GetGenericArguments()[0];
                }
            }
            var name = tableName ?? GetTableName(type);
            var sbColumnList = new StringBuilder(null);
            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type).ToList();
            var computedProperties = ComputedPropertiesCache(type);
            var allPropertiesExceptKeyAndComputed = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                sbColumnList.AppendFormat("`{0}`", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbParameterList.Append(", ");
            }

            //insert list of entities
            var cmd = $"INSERT INTO {name} ({sbColumnList}) values ({sbParameterList})";
            var result = await connection.ExecuteAsync(cmd, entityToInsert, transaction, commandTimeout);
            if (!isList && keyProperties.Count == 1)
            {
                var lastId = await connection.QueryAsync("SELECT LAST_INSERT_ID() id", transaction: transaction, commandTimeout: commandTimeout);
                var id = lastId.First().id;
                if (id == null)
                {
                    return result;
                }
                var key = keyProperties.First();
                key.SetValue(entityToInsert, Convert.ChangeType(id, key.PropertyType), null);
            }
            return result;
        }


        /// <summary>
        /// Updates entity in table "Ts" asynchronously using Task, checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static Task<bool> UpdateAsync<T>(this IDbConnection connection, T entityToUpdate, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return UpdateAsync(connection, typeof(T), entityToUpdate, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// Updates entity in table "Ts" asynchronously using Task, checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <param name="type">Type to be updated</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static async Task<bool> UpdateAsync(this IDbConnection connection, Type type, object entityToUpdate, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if ((entityToUpdate is IProxy proxy) && !proxy.IsDirty)
            {
                return false;
            }
            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }

            var keyProperties = KeyPropertiesCache(type).ToList();
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            var name = tableName ?? GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);

            var allProperties = TypePropertiesCache(type);
            keyProperties.AddRange(explicitKeyProperties);
            var computedProperties = ComputedPropertiesCache(type);
            var nonIdProps = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                sb.AppendFormat("`{0}` = @{1}", property.Name, property.Name);
                if (i < nonIdProps.Count - 1)
                    sb.Append(", ");
            }
            sb.Append(" where ");
            for (var i = 0; i < keyProperties.Count; i++)
            {
                var property = keyProperties[i];
                sb.AppendFormat("`{0}` = @{1}", property.Name, property.Name);
                if (i < keyProperties.Count - 1)
                    sb.Append(" and ");
            }
            var updated = await connection.ExecuteAsync(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction).ConfigureAwait(false);
            return updated > 0;
        }

        /// <summary>
        /// Inserts or update an entity into table "Ts" asynchronously using Task, checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">The type being inserted or updated.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entity">Entity to insert</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Identity of inserted entity</returns>
        public static Task<bool> InsertOrUpdateAsync<T>(this IDbConnection connection, object entity, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return InsertOrUpdateAsync(connection, typeof(T), entity, tableName, transaction, commandTimeout);
        }

        /// <summary>
        /// Inserts or update an entity into table "Ts" asynchronously using Task, checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <param name="type">The type being inserted or updated.</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entity">Entity to insert</param>
        /// <param name="tableName"></param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Identity of inserted entity</returns>
        public static async Task<bool> InsertOrUpdateAsync(this IDbConnection connection, Type type, object entity, string tableName = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {

            if ((entity is IProxy proxy) && !proxy.IsDirty)
            {
                return false;
            }
            var isList = false;
            if (type.IsArray)
            {
                isList = true;
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                var implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    isList = true;
                    type = type.GetGenericArguments()[0];
                }
            }
            var name = tableName ?? GetTableName(type);
            var sbColumnList = new StringBuilder(null);
            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type);
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);

            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            var computedProperties = ComputedPropertiesCache(type);
            var allPropertiesExceptKeyAndComputed = allProperties.Except(keyProperties.Union(computedProperties)).ToList();
            var nonIdProps = allPropertiesExceptKeyAndComputed.Except(keyProperties.Union(explicitKeyProperties)).ToList();
            var key = keyProperties.SingleOrDefault();
            if (!IsDefaultValue(key.PropertyType, key.GetValue(entity)))
            {
                allPropertiesExceptKeyAndComputed.Insert(0, key);
            }
            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                sbColumnList.AppendFormat("`{0}`", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbParameterList.Append(", ");
            }

            var sb = new StringBuilder(null);
            if (key != null)
            {
                sb.AppendFormat("`{0} = LAST_INSERT_ID(`{0}`),", key.Name);
            }
            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                sb.AppendFormat("`{0}`", property.Name);
                sb.Append("=values(");
                sb.AppendFormat("`{0}`", property.Name);
                sb.Append(")");
                if (i < nonIdProps.Count - 1)
                    sb.Append(", ");
            }
            //insert list of entities
            var cmd = $"insert into `{name}` ({sbColumnList}) values ({sbParameterList}) ON DUPLICATE KEY UPDATE {sb};";
            var result = await connection.ExecuteAsync(cmd, entity, transaction, commandTimeout);
            if (!isList && keyProperties.Count == 1 && result == 1 && !IsDefaultValue(key.PropertyType, key.GetValue(entity)))
            {
                var lastId = await connection.QueryAsync("SELECT LAST_INSERT_ID() id", transaction: transaction, commandTimeout: commandTimeout);
                var id = lastId.First().id;
                if (id == null)
                {
                    return true;
                }
                key.SetValue(entity, Convert.ChangeType(id, key.PropertyType), null);
            }
            return true;
        }

        /// <summary>
        /// Delete entity in table "Ts" asynchronously using Task.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static Task<bool> DeleteAsync<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return DeleteAsync(connection, typeof(T), entityToDelete, transaction, commandTimeout);
        }

        /// <summary>
        /// Delete entity in table "Ts" asynchronously using Task.
        /// </summary>
        /// <param name="type">Type of entity</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static async Task<bool> DeleteAsync(this IDbConnection connection,Type type, object entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (entityToDelete == null)
                throw new ArgumentException("Cannot Delete null Object", nameof(entityToDelete));

            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }

            var keyProperties = KeyPropertiesCache(type);
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            var name = GetTableName(type);
            var allKeyProperties = keyProperties.Concat(explicitKeyProperties).ToList();

            var sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM {0} WHERE ", name);

            for (var i = 0; i < allKeyProperties.Count; i++)
            {
                var property = allKeyProperties[i];
                sb.AppendFormat("`{0}` = @{1}", property.Name, property.Name);
                if (i < allKeyProperties.Count - 1)
                    sb.Append(" AND ");
            }
            var deleted = await connection.ExecuteAsync(sb.ToString(), entityToDelete, transaction, commandTimeout).ConfigureAwait(false);
            return deleted > 0;
        }
    }
}
