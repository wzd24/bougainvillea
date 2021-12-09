using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Essential
{

    /// <summary>
    /// 
    /// </summary>
    public class AvatarOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ITypeList<ISubSystem>  SubSystems { get;  }

        /// <summary>
        /// 
        /// </summary>
        public AvatarOptions()
        {
            SubSystems = new TypeList<ISubSystem>();
        }
    }
}
