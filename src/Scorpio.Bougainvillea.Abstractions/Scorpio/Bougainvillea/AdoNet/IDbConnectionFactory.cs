using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.AdoNet
{

    /// <summary>
    /// AdoNet连接字符串工厂接口。
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// 根据服务器Id及连接字符串获取数据库连接。
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        Task<IDbConnection> GetDbConnectionAsync(int connectionId, string connectionName);
    }
}
