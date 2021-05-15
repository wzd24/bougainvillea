﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Contrib.Extensions;

using Microsoft.Extensions.DependencyInjection;

using Scorpio;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{


    internal class MySQLGameSettingStore : IGameSettingStore, ISingletonDependency
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MySQLGameSettingStore(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<GameSettingValue<T>> GetAsync<T>(IGameSettingStoreContext context)
            where T : class
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                using (var conn = await GetConnectionAsync(context, scope.ServiceProvider))
                {
                    var result = await conn.QueryAsync<T>($"SELECT * FROM {context.SettingDefinition.Name}");
                    return new GameSettingValue<T> { Definition = context.SettingDefinition, Value = result.ToDictionary(r => r.ToString()) };
                }
            }
        }

        private async Task<IDbConnection> GetConnectionAsync(IGameSettingStoreContext context, IServiceProvider serviceProvider)
        {
            var providers = serviceProvider.GetServices<IGameSettingDatabaseConnectionProvider>();
            foreach (var item in providers)
            {
                var conn = await item.GetConnectionAsync(context.SettingDefinition);
                if (conn != null)
                {
                    return conn;
                }
            }
            throw new ScorpioException("未找到数据库连接！");
        }

        public async Task SetAsync<T>(IGameSettingStoreContext context, T value)
            where T : class
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                using (var conn = await GetConnectionAsync(context, scope.ServiceProvider))
                {
                    await conn.InsertAsync(value);
                }
            }
        }
    }
}
