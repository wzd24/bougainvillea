using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Orleans.Storage;

using Scorpio.Bougainvillea.Storages;
using Scorpio.Conventional;

namespace Scorpio.Bougainvillea
{
    internal class ConventionRegistrar : IConventionalRegistrar
    {
        public void Register(IConventionalRegistrationContext context)
        {
            context.DoConventionalAction<StroageRegisterConventionalAction>(config 
                => config.Where(t => t.IsStandardType()).Where(t => t.IsAssignableTo<IGrainStorage>() && t.AttributeExists<StroageNameAttribute>()));
        }
    }
}
