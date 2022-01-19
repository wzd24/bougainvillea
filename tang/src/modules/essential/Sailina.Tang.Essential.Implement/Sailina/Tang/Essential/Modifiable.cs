
using Scorpio.Bougainvillea.Data;

namespace Sailina.Tang.Essential
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
