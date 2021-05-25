using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans.Runtime;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Props.Settings;
using Scorpio.Bougainvillea.Setting;

using Serilog.Context;

namespace Scorpio.Bougainvillea.Props
{
    internal class PropsHandleManager : IPropsHandleManager
    {
        private readonly Lazy<IEnumerable<IPropsHandlerProvider>> _providers;
        private readonly PropsHandleOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPropsSet _propsSet;
        private readonly ICurrentUser _currentUser;
        private readonly IGameSettingManager _settingManager;
        private readonly ILogger<PropsHandleManager> _logger;

        public IEnumerable<IPropsHandlerProvider> Providers => _providers.Value;

        public PropsHandleManager(IServiceProvider serviceProvider,
                                  IOptions<PropsHandleOptions> options,
                                  IPropsSet propsSet,
                                  ICurrentUser currentUser,
                                  IGameSettingManager settingManager,
                                  ILogger<PropsHandleManager> logger)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _propsSet = propsSet;
            _currentUser = currentUser;
            _settingManager = settingManager;
            _logger = logger;
            _providers = new Lazy<IEnumerable<IPropsHandlerProvider>>(() =>
                  _options.HandlerProviders.Select(t => _serviceProvider.GetService(t) as IPropsHandlerProvider), true);
        }

        public async Task<(int code, object data)> AddPropAsync(int propId, int num, string reason)
        {
            if (num == 0)
                return (PropsErrorCodes.ExceptionParameter, null);
            var (handled, code, data) = await Handle<IPropsAddHandler>(propId, num, null, (h, c) => h.AddPropAsync(c));
            if (!handled)
            {
                var setting = (await _settingManager.GetAsync<PropsSetting>()).GetOrDefault(propId);
                if (setting == null)
                {
                    (code, data) = (PropsErrorCodes.NotExist, null);
                }
                else
                {
                    (code, data) = await _propsSet.AddOrSubtractAsync(propId, num);
                }
            }
            _logger.LogInformation("玩家{ServerId}-{AvatarId} 添加数量为 {Num}的道具 {PropId},添加原因：{Reason}，添加返回结果：{@Result}", _currentUser.ServerId, _currentUser.Id, num, propId, reason, (code, data));
            return (code, data);
        }

        public async Task<(int code, object data)> CanUseAsync(int propId, int num, object para = null)
        {
            var (code, props) = await EnoughAsync(propId, num);
            if (code != 0)
            {
                return (code, props);
            }
            var (handled, cd, data) = await Handle<IPropsCanUseHandler>(propId, num, para, async (h, c) => await h.CanUseAsync(c));
            if (handled)
            {
                return (cd, data as Props);
            }
            var setting = (await _settingManager.GetAsync<PropsSetting>()).GetOrDefault(propId);
            if (setting.UseType == UseType.CanNotBeUsedDirectly || setting.UseType == UseType.CanNotUse)
            {
                return (PropsErrorCodes.NotCanUse, null);
            }
            return (0, data as Props);
        }

        private async Task<(int code, object data)> ConsumeAsync(int propId, int num)
        {
            var (code, data) = await EnoughAsync(propId, num);
            if (code == 0)
            {
                var (handled, c, d) = await Handle<IPropsConsumeHandler>(propId, num, null, (h, c) => h.ConsumeAsync(c));
                if (!handled)
                {
                    (code, data) = await _propsSet.AddOrSubtractAsync(propId, num);
                }
                else
                {
                    code = c;
                    data = d;
                }
            }
            return (code, data);
        }

        public async Task<(int code, object data)> ConsumeAsync(int propId, int num, string reason)
        {
            var (code, data) =await ConsumeAsync(propId, num);
            _logger.LogInformation("玩家{ServerId}-{AvatarId} 消费数量为 {Num}的道具 {PropId},消费原因：{Reason}，消费返回结果：{@Result}", _currentUser.ServerId, _currentUser.Id, num, propId, reason, (code, data));
            return (code, data);
        }


        public async Task<(int code, object data)> EnoughAsync(int propId, int num)
        {
            if (num == 0)
                return (PropsErrorCodes.ExceptionParameter, null);
            var (handled, code, data) = await Handle<IPropsEnoughHandler>(propId, num, null, async (h, c) => await h.EnoughAsync(c));
            if (handled)
            {
                return (code, data);
            }
            var setting = (await _settingManager.GetAsync<PropsSetting>()).GetOrDefault(propId);
            if (setting == null)
            {
                return (PropsErrorCodes.NotExist, null);
            }
            var prop = await _propsSet.GetPropsAsync(propId);
            var count = (prop?.Count) ?? 0;
            if (count<=0)
            {
                return (PropsErrorCodes.NotHave, new { PropId = propId, Expect = num, Actual = count });
            }
            if (count < num)
            {
                return (PropsErrorCodes.NotEnough, new { PropId = propId, Expect = num, Actual = count });
            }
            return (0, new { PropId = propId, Expect = num, Actual = count });
        }


        public async Task<(int code, object data)> UseAsync(int propId, int num, string reason, object para = null)
        {
            var (code, data) = await CanUseAsync(propId, num, para);
            if (code == 0)
            {
                (code, data) = await ConsumeAsync(propId, num);
            }
            if (code == 0)
            {
                (_, code, data) = await Handle<IPropsHandler>(propId, num, para, (h, c) => h.UseAsync(c));
            }
            _logger.LogInformation("玩家{ServerId}-{AvatarId} 使用数量为 {Num}道具 {PropId},使用原因：{Reason}，附加参数：{@Parameter},使用返回结果：{@Result}", _currentUser.ServerId, _currentUser.Id, num, propId, reason,para, (code, data));
            return (code, data);
        }

        private async Task<(bool handled, int code, object data)> Handle<T>(int propId, int num, object parameter, Func<T, PropsHandleContext, Task<(int code, object data)>> action)
           where T : class
        {
            if (GetHandler(propId) is not T handler)
            {
                return (false, 0, null);
            }
            var context = CreateContext(propId, num, parameter);
            var (code, data) = await action(handler, context);
            return (true, code, data);

        }
        private PropsHandleContext CreateContext(int propId, int num, object parameter)
        {
            return new PropsHandleContext { PropId = propId, Num = num, Parameter = parameter };
        }
        private IPropsHandler GetHandler(int propId)
        {
            var handler = Providers.Reverse()
                                    .Select(provider => provider.GetHandler(propId))
                                    .FirstOrDefault(h => h != null) ?? throw new NullReferenceException("未找到对应的道具处理器");
            return handler;
        }
    }
}
