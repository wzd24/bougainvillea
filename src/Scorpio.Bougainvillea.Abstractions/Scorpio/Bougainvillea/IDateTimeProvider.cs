using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        ValueTask<DateTimeOffset> GetNowAsync();
    }
}
