using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityBase<TEntity> : IDirtyEntity<TEntity>, ISupportInitialize
        where TEntity : EntityBase<TEntity>
    {
        private TEntity _original;
        TEntity IDirtyEntity<TEntity>.Original => _original;
        PropertyInfo[] IDirtyEntity<TEntity>.DirtyProperies
        {
            get
            {
                var properies = GetSettableProps(typeof(TEntity));
                return properies.Where(p => !p.GetValue(_original).Equals(p.GetValue(this))).ToArray();
            }
        }

        void ISupportInitialize.BeginInit()
        {

        }

        void ISupportInitialize.EndInit()
        {
            _original = MemberwiseClone().As<TEntity>();
        }

        void IDirtyEntity<TEntity>.Flush()
        {
            _original = MemberwiseClone().As<TEntity>();
        }

        internal static MethodInfo GetPropertySetter(PropertyInfo propertyInfo, Type type)
        {
            if (propertyInfo.DeclaringType == type) return propertyInfo.GetSetMethod(true);

            return propertyInfo.DeclaringType.GetProperty(
                   propertyInfo.Name,
                   BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                   Type.DefaultBinder,
                   propertyInfo.PropertyType,
                   propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray(),
                   null).GetSetMethod(true);
        }

        internal static List<PropertyInfo> GetSettableProps(Type t)
        {
            return t
                  .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                  .Where(p => GetPropertySetter(p, t) != null)
                  .ToList();
        }
    }
}
