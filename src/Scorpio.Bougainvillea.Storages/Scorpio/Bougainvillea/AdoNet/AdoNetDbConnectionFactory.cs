using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Scorpio.Bougainvillea.Setting;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.AdoNet
{

    internal class AdoNetDbConnectionFactory : IDbConnectionFactory, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public AdoNetDbConnectionFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<IDbConnection> GetDbConnectionAsync(int connectionId, string connectionName)
        {
            var configuration = _serviceProvider.GetService<IConfiguration>();
            if (connectionId != 0)
            {
                var settings = await _serviceProvider.GetService<IGameSettingManager>().GetAsync<ConnectionStringSetting>();
                var setting = settings.Values.FirstOrDefault(s => s.ServerId == connectionId && s.ConnectionName == connectionName);
                if (setting != null)
                {
                    return DbConnectionFactory.CreateConnection(setting.ConnectionString);
                }
            }
            return DbConnectionFactory.CreateConnection(configuration.GetConnectionString(connectionName));
        }
    }
}
