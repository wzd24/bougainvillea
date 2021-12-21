using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyPersistentStateAttribute : Attribute, IFacetMetadata, IPersistentStateConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string StateName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string StorageName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="storageName"></param>
        public PropertyPersistentStateAttribute(string stateName, string storageName = null)
        {
            StateName = stateName;
            StorageName = storageName;
        }
    }
}
