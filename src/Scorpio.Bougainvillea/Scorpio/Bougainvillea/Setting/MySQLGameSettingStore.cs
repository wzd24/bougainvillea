using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Extensions;

using Microsoft.Extensions.DependencyInjection;

using Scorpio;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Setting
{


    internal class MySQLGameSettingStore : IGameSettingStore, ISingletonDependency
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly MethodInfo _getMethod = typeof(MySQLGameSettingStore).GetMethod(nameof(Invoke));
        public MySQLGameSettingStore(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<GameSettingValue> GetAsync(IGameSettingStoreContext context)
        {
            var method=_getMethod.MakeGenericMethod(context.SettingDefinition.ValueType);
            var result=method.Invoke(this, new object[] {context}) as Task<GameSettingValue>;
            return await result;
        }
        public async Task<GameSettingValue> Invoke<T>(IGameSettingStoreContext ctx)
         where T : GameSettingBase
        {
            return await GetAsync<T>(ctx);
        }

        public async Task<GameSettingValue<T>> GetAsync<T>(IGameSettingStoreContext context)
            where T : GameSettingBase
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                using (var conn = await GetConnectionAsync(context, scope.ServiceProvider))
                {
                    var result = await conn.GetAllAsync<T>(tableName: context.SettingDefinition.Name);
                    return new GameSettingValue<T>(result.ToHashSet()) { Definition = context.SettingDefinition };
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
            where T : GameSettingBase
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                using (var conn = await GetConnectionAsync(context, scope.ServiceProvider))
                {
                    await conn.InsertOrUpdateAsync<T>(value, tableName: context.SettingDefinition.Name);
                }
            }
        }
    }
}
