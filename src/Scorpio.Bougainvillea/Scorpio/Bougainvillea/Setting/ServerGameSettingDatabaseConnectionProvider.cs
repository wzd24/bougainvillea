using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.AdoNet;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{
    internal class ServerGameSettingDatabaseConnectionProvider : IGameSettingDatabaseConnectionProvider, ISingletonDependency
    {
        private readonly ICurrentServer _currentServer;
        private readonly IDbConnectionFactory _connectionFactory;

        public ServerGameSettingDatabaseConnectionProvider(ICurrentServer currentServer, IDbConnectionFactory connectionFactory)
        {

            _currentServer = currentServer;
            _connectionFactory = connectionFactory;
        }
        public async Task<IDbConnection> GetConnectionAsync(GameSettingDefinition settingDefinition)
        {
            if (settingDefinition.Scope == GameSettingScope.Server)
            {
                return await CreateConnection();
            }
            return null;
        }

        private async Task<IDbConnection> CreateConnection()
        {
            var conn = await _connectionFactory.GetDbConnectionAsync(_currentServer.ServerId, ConnectionStringName.GameSettingDatabaseConnectionString);
            return conn;
        }
    }
}
