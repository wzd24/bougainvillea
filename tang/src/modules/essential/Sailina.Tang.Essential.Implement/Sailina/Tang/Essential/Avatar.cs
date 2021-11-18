using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Avatar : AvatarBase<Avatar, AvatarState,AvatarEntity>, IAvatar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public Avatar(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<string> GetAvatarNameAsync() => Task.FromResult("Test");
    }

}
