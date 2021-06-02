using Microsoft.Extensions.DependencyInjection;
using Scorpio.Bougainvillea.Setting;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Conventional;
using System.Reflection;
using Scorpio.Bougainvillea.Rewards;
using Scorpio.Bougainvillea.Depletion;
using Scorpio.Setting;

namespace Scorpio.Bougainvillea
{
    internal class ConventionRegistrar : IConventionalRegistrar
    {
        public void Register(IConventionalRegistrationContext context)
        {
            context.DoConventionalAction<ConventionalAttributeRewardHandlerRegisterAction>(config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.AttributeExists<RewardHandlerAttribute>());
            });
            context.DoConventionalAction<ConventionalAttributeDepleteHandlerRegisterAction>(config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.AttributeExists<DepleteHandlerAttribute>());
            });
            context.DoConventionalAction<ConventionalGameSettingDefinitionProviderRegisterAction>(config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<IGameSettingDefinitionProvider>());
            });
            context.DoConventionalAction<ConventionalSettingDefinitionProviderRegisterAction>(config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<ISettingDefinitionProvider>());
            });
            context.Services.DoConventionalAction<ConventionalHandlersRegisterAction>(context.Types, config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<IGameHandler>() || t.AttributeExists<HandlerAttribute>());
            });
        }
    }
}
