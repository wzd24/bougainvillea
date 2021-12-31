using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EasyMigrator;

namespace Dapper.Extensions
{
    public partial class SqlMapperExtensions
    {
        #region Private members
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> _keyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> _explicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> _typeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> _computedProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> _getQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> _typeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static List<PropertyInfo> ComputedPropertiesCache(Type type)
        {
            if (_computedProperties.TryGetValue(type.TypeHandle, out var pi))
            {
                return pi.ToList();
            }

            var computedProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            _computedProperties[type.TypeHandle] = computedProperties;
            return computedProperties;
        }


        private static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
        {
            if (_explicitKeyProperties.TryGetValue(type.TypeHandle, out var pi))
            {
                return pi.ToList();
            }

            var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();
            _explicitKeyProperties[type.TypeHandle] = explicitKeyProperties;
            return explicitKeyProperties;
        }

        private static List<PropertyInfo> KeyPropertiesCache(Type type)
        {
            if (_keyProperties.TryGetValue(type.TypeHandle, out var pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();


            _keyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }

        private static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            if (_typeProperties.TryGetValue(type.TypeHandle, out var pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(IsWriteable).ToArray();
            _typeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }

        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1) return true;
            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        private static string GetTableName(Type type)
        {
            if (_typeTableName.TryGetValue(type.TypeHandle, out var name)) return name;

            //NOTE: This as dynamic trick falls back to handle both our own Table-attribute as well as the one in EntityFramework 
            var tableAttrName =
                type.GetCustomAttribute<TableAttribute>(false)?.Name
                ?? type.GetCustomAttribute<NameAttribute>(false)?.Name;

            if (tableAttrName != null)
            {
                name = tableAttrName;
            }
            else
            {
                name = type.Name;
                if (type.IsInterface)
                    name = name.RemovePreFix("I");
            }
            _typeTableName[type.TypeHandle] = name;
            return name;
        }

        private static PropertyInfo[] GetKeys(Type type, string method)
        {
            var keys = KeyPropertiesCache(type);
            var explicitKeys = ExplicitKeyPropertiesCache(type);
            if (keys.Count + explicitKeys.Count == 0)
                throw new DataException($"{method}<T> only supports an entity with a [Key] or an [ExplicitKey] property");
            return keys.Concat(explicitKeys).ToArray();
        }


        private static async Task<IEnumerable<object>> GetAllAsyncImpl(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string sql, Type type, object parameters = null)
        {
            var result = await connection.QueryAsync(sql, param: parameters, transaction: transaction, commandTimeout: commandTimeout).ConfigureAwait(false);
            var list = new List<object>();
            foreach (IDictionary<string, object> res in result)
            {
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
                list.Add(obj);
            }
            return list;
        }

        private static bool IsDefaultValue(Type t, object value)
        {
            var @default = GetDefaultValue(t);
            return value.Equals(@default);
        }

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
        #endregion

    }
}
