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
        public bool Modified { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void ResetModifyState()=>Modified = false;

        /// <summary>
        /// 
        /// </summary>
        public void SetModifyState()=>Modified=true;
    }
}
