using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Bougainvillea.Props.Settings;
using Scorpio.Bougainvillea.Setting;
using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea.Props
{
    internal class PropsHandleManager : IPropsHandleManager,ITransientDependency
    {
        private readonly Lazy<IEnumerable<IPropsHandlerProvider>> _providers;
        private readonly PropsHandleOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPropsSet _propsSet;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentServer _currentServer;
        private readonly IGameSettingManager _settingManager;
        private readonly ILogger<PropsHandleManager> _logger;

        public IEnumerable<IPropsHandlerProvider> Providers => _providers.Value;

        public PropsHandleManager(IServiceProvider serviceProvider,
                                  IOptions<PropsHandleOptions> options,
                                  IPropsSet propsSet,
                                  ICurrentUser currentUser,
                                  ICurrentServer currentServer,
                                  IGameSettingManager settingManager,
                                  ILogger<PropsHandleManager> logger)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _propsSet = propsSet;
            _currentUser = currentUser;
            _currentServer = currentServer;
            _settingManager = settingManager;
            _logger = logger;
            _providers = new Lazy<IEnumerable<IPropsHandlerProvider>>(() =>
                  _options.HandlerProviders.Select(t => _serviceProvider.GetService(t) as IPropsHandlerProvider), true);
        }

        public async Task<int> AddPropAsync(int propId, int num, string reason)
        {
            if (num == 0)
                return PropsErrorCodes.ExceptionParameter;
            var (handled, code, _) = await Handle<IPropsAddHandler>(propId, num, null, async (h, c) => (await h.AddPropAsync(c), null));
            if (!handled)
            {
                var setting = (await _settingManager.GetAsync<PropsSetting>()).FirstOrDefault(p=>p.Id==propId);
                if (setting == null)
                {
                    code = PropsErrorCodes.NotExist;
                }
                else
                {
                    code = await _propsSet.AddOrSubtractAsync(propId, num);
                }
            }
            _logger.LogInformation("玩家{ServerId}-{AvatarId} 添加数量为 {Num}的道具 {PropId},添加原因：{Reason}，添加返回结果：{@Result}", _currentServer.ServerId, _currentUser.AvatarId, num, propId, reason, code);
            return code;
        }

        public async Task<int> CanUseAsync(int propId, int num, object para = null)
        {
            var code = await EnoughAsync(propId, num);
            if (code != 0)
            {
                return code;
            }
            var (handled, cd, _) = await Handle<IPropsCanUseHandler>(propId, num, para, async (h, c) => (await h.CanUseAsync(c), null));
            if (handled)
            {
                return cd;
            }
            var setting = (await _settingManager.GetAsync<PropsSetting>()).FirstOrDefault(p => p.Id == propId);
            if (setting.UseType == UseType.CanNotBeUsedDirectly || setting.UseType == UseType.CanNotUse)
            {
                return PropsErrorCodes.NotCanUse;
            }
            return SystemErrorCodes.Success;
        }

        private async Task<int> ConsumeAsync(int propId, int num)
        {
            var code = await EnoughAsync(propId, num);
            if (code != 0)
            {
                return code;
            }
            var (handled, c, _) = await Handle<IPropsConsumeHandler>(propId, num, null, async (h, c) => (await h.ConsumeAsync(c), null));
            if (handled)
            {
                return c;
            }
            return await _propsSet.AddOrSubtractAsync(propId, -num);
        }

        public async Task<int> ConsumeAsync(int propId, int num, string reason)
        {
            var code = await ConsumeAsync(propId, num);
            _logger.LogInformation("玩家{ServerId}-{AvatarId} 消费数量为 {Num}的道具 {PropId},消费原因：{Reason}，消费返回结果：{@Result}", _currentServer.ServerId, _currentUser.AvatarId, num, propId, reason, code);
            return code;
        }


        public async Task<int> EnoughAsync(int propId, int num)
        {
            if (num == 0)
                return PropsErrorCodes.ExceptionParameter;
            var (handled, code, data) = await Handle<IPropsEnoughHandler>(propId, num, null, async (h, c) => (await h.EnoughAsync(c), null));
            if (handled)
            {
                return code;
            }
            var setting = (await _settingManager.GetAsync<PropsSetting>()).FirstOrDefault(p => p.Id == propId);
            if (setting == null)
            {
                return PropsErrorCodes.NotExist;
            }
            var prop = await _propsSet.GetPropsAsync(propId);
            var count = (prop?.Count) ?? 0;
            if (count <= 0)
            {
                return PropsErrorCodes.NotHave;
            }
            if (count < num)
            {
                return PropsErrorCodes.NotEnough;
            }
            return SystemErrorCodes.Success;
        }


        public async Task<(int code, object data)> UseAsync(int propId, int num, string reason, object para = null)
        {
            var code = await CanUseAsync(propId, num, para);
            object data = null;
            if (code == 0)
            {
                code = await ConsumeAsync(propId, num);
            }
            if (code == 0)
            {
                (_, code, data) = await Handle<IPropsHandler>(propId, num, para, (h, c) => h.UseAsync(c));
            }
            _logger.LogInformation("玩家{ServerId}-{AvatarId} 使用数量为 {Num}道具 {PropId},使用原因：{Reason}，附加参数：{@Parameter},使用返回结果：{@Result}", _currentServer.ServerId, _currentUser.AvatarId, num, propId, reason, para, (code, data));
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
