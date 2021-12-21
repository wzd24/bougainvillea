using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper.Extensions;

namespace Scorpio.Bougainvillea.Setting
{

    /// <summary>
    /// 游戏配置基类
    /// </summary>
    public abstract class GameSettingBase
    {
        /// <summary>
        /// 序号（根据策划配置）
        /// </summary>
        [ExplicitKey]
        public int Id { get; set; }

    }
}
