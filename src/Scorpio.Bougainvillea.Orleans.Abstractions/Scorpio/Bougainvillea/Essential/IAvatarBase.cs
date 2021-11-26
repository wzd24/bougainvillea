using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using Scorpio.Bougainvillea.Essential.Dtos;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface IAvatarBase : IGrainWithIntegerKey, IGrainBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        ValueTask<int> GenerateAsync(GenerateInfo generateInfo);
    }
}
