using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;


namespace EasyMigrator.Parsing
{

    /// <summary>
    /// 
    /// </summary>
    public interface ITypeMap
    {

        /// <summary>
        /// 
        /// </summary>
        DbType this[PropertyInfo field] { get; }

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(Type underlyingType, DbType dbType);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(IEnumerable<Type> underlyingTypes, DbType dbType);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(IDictionary<Type, DbType> map);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(IDictionary<IEnumerable<Type>, DbType> map);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(IEnumerable<Type> underlyingTypes, Func<PropertyInfo, DbType> dbTypeProvider);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(IDictionary<Type, Func<PropertyInfo, DbType>> providerMap);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Add(IDictionary<IEnumerable<Type>, Func<PropertyInfo, DbType>> providerMap);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Clone();


        /// <summary>
        /// 
        /// </summary>
        ITypeMap Remove(Type underlyingType);


        /// <summary>
        /// 
        /// </summary>
        ITypeMap Replace(Type underlyingType, DbType dbType);

        /// <summary>
        /// 
        /// </summary>
        ITypeMap Replace(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider);
    }


    /// <summary>
    /// 
    /// </summary>
    public class TypeMap : ITypeMap
    {
        private class ProviderPair
        {
            private readonly DbType _dbType;
            private readonly Func<PropertyInfo, DbType> _dbTypeProvider = null;

            public ProviderPair(DbType dbType) { _dbType = dbType; }
            public ProviderPair(Func<PropertyInfo, DbType> dbTypeProvider) { _dbTypeProvider = dbTypeProvider; }

            public DbType GetDbType(PropertyInfo field) { return _dbTypeProvider == null ? _dbType : _dbTypeProvider(field); }
        }

        private readonly Dictionary<Type, ProviderPair> _map;


        /// <summary>
        /// 
        /// </summary>
        public TypeMap() : this(new Dictionary<Type, ProviderPair>()) { }
        private TypeMap(Dictionary<Type, ProviderPair> map) { _map = map; }


        /// <summary>
        /// 
        /// </summary>
        public DbType this[PropertyInfo field]
        {
            get {
                var type = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;
                if (!_map.ContainsKey(type))
                    throw new Exception("No DbType mapped to native type " + type.Name);

                return _map[type].GetDbType(field);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Clone() => new TypeMap(new Dictionary<Type, ProviderPair>(_map));

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(Type underlyingType, DbType dbType) { Add(underlyingType, new ProviderPair(dbType)); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(IEnumerable<Type> underlyingTypes, DbType dbType) { foreach (var underlyingType in underlyingTypes) Add(underlyingType, dbType); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(IDictionary<Type, DbType> map) { foreach (var kv in map) Add(kv.Key, kv.Value); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(IDictionary<IEnumerable<Type>, DbType> map) { foreach (var kv in map) Add(kv.Key, kv.Value); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider) { Add(underlyingType, new ProviderPair(dbTypeProvider)); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(IEnumerable<Type> underlyingTypes, Func<PropertyInfo, DbType> dbTypeProvider) { foreach (var underlyingType in underlyingTypes) Add(underlyingType, dbTypeProvider); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(IDictionary<Type, Func<PropertyInfo, DbType>> providerMap) { foreach (var kv in providerMap) Add(kv.Key, kv.Value); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Add(IDictionary<IEnumerable<Type>, Func<PropertyInfo, DbType>> providerMap) { foreach (var kv in providerMap) Add(kv.Key, kv.Value); return this; }

        private void Add(Type underlyingType, ProviderPair providerPair)
        {
            if (_map.ContainsKey(underlyingType))
                throw new Exception("Native type '" + underlyingType.Name + "' is already mapped.");
            _map.Add(underlyingType, providerPair);
        }



        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Remove(Type underlyingType)
        {
            if (_map.ContainsKey(underlyingType))
                _map.Remove(underlyingType);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Replace(Type underlyingType, DbType dbType) { Replace(underlyingType, new ProviderPair(dbType)); return this; }

        /// <summary>
        /// 
        /// </summary>
        public ITypeMap Replace(Type underlyingType, Func<PropertyInfo, DbType> dbTypeProvider) { Replace(underlyingType, new ProviderPair(dbTypeProvider)); return this; }

        private void Replace(Type underlyingType, ProviderPair providerPair)
        {
            if (_map.ContainsKey(underlyingType))
                _map[underlyingType] = providerPair;
            else
                _map.Add(underlyingType, providerPair);
        }
    }
}
