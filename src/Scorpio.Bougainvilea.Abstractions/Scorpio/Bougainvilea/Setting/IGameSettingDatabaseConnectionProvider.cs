using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Setting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameSettingDatabaseConnectionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingDefinition"></param>
        /// <returns></returns>
        Task<IDbConnection> GetConnectionAsync(GameSettingDefinition settingDefinition);
    }
}
