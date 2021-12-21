using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Storages
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class StroageNameAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="storageName"></param>
        public StroageNameAttribute(string storageName)
        {
            StorageName = storageName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string StorageName { get; }
    }
}
