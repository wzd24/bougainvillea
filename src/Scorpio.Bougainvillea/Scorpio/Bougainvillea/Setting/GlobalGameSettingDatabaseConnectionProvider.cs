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
    internal class GlobalGameSettingDatabaseConnectionProvider : IGameSettingDatabaseConnectionProvider, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public GlobalGameSettingDatabaseConnectionProvider(IServiceProvider serviceProvider, IConfiguration configuration)
        {

            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }
        public async Task<IDbConnection> GetConnectionAsync(GameSettingDefinition settingDefinition)
        {
            if (settingDefinition.Scope == GameSettingScope.Global)
            {
                return await CreateConnection();
            }
            return null;
        }

        private Task<IDbConnection> CreateConnection()
        {
            var conn = DbConnectionFactory.CreateConnection(_configuration.GetConnectionString("Game_Config"));
            return Task.FromResult(conn);
        }

    }
}
