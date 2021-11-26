using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModifiable
    {
        /// <summary>
        /// 
        /// </summary>
        bool Modified { get;}

        /// <summary>
        /// 
        /// </summary>
        void ResetModifyState();
    }
}
