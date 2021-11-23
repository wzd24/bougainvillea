using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans.Runtime;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential
{
    /// <summary>
    /// 
    /// </summary>
    internal partial class Avatar : AvatarBase<Avatar, AvatarState,AvatarBase>, IAvatar
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
        public Task<string> GetAvatarNameAsync()
        {
            return Task.FromResult(State.Base.NickName);
        }
    }

}
