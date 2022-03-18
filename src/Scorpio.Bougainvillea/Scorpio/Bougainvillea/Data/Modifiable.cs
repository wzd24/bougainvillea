using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Data
{
    internal class Modifiable : IModifiable
    {
        public bool Modified { get; set; }

        public void ResetModifyState() => Modified = false;

        /// <summary>
        /// 
        /// </summary>
        public void SetModifyState() => Modified = true;

    }
}
