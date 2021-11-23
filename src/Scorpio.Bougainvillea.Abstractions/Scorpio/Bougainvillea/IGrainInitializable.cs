using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGrainInitializable:ISingletonDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ValueTask InitializeAsync();
    }
}
