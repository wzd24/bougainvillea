﻿using System;
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
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IConfiguration _configuration;

        public GlobalGameSettingDatabaseConnectionProvider(IDbConnectionFactory connectionFactory, IConfiguration configuration)
        {

            _connectionFactory = connectionFactory;
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

        private async Task<IDbConnection> CreateConnection()
        {
            var conn =await _connectionFactory.GetDbConnectionAsync(0,ConnectionStringName.GameSettingDatabaseConnectionString);
            return conn;
        }
    }
}
