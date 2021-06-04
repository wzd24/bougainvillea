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
    public interface IHasExtraProperties
    {
        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, object> ExtraProperties { get; }
    }
}
