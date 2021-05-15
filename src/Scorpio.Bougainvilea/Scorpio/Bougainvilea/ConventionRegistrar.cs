using Microsoft.Extensions.DependencyInjection;
using Scorpio.Bougainvillea.Setting;

using Scorpio.Bougainvillea.Handler;
using Scorpio.Conventional;
using System.Reflection;

namespace Scorpio.Bougainvillea
{
    internal class ConventionRegistrar : IConventionalRegistrar
    {
        public void Register(IConventionalRegistrationContext context)
        {
            context.DoConventionalAction<ConventionalSettingDefinitionProviderRegisterAction>(config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<IGameSettingDefinitionProvider>());
            });
            context.Services.DoConventionalAction<ConventionalHandlersRegisterAction>(context.Types, config =>
            {
                config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<IGameHandler>() || t.AttributeExists<HandlerAttribute>());
            });
        }
    }
}
