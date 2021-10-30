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
    public interface IAvatarBase: IGrainWithIntegerKey
    {
        /// <summary>
        /// 角色是否已创建
        /// </summary>
        /// <returns></returns>
        Task<bool> IsGenerated();

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="generateInfo"></param>
        /// <returns></returns>
        Task<int> Generate(GenerateInfo generateInfo);

        /// <summary>
        /// 创建角色后操作
        /// </summary>
        /// <returns></returns>
        Task PostGenerate();
    }
}
