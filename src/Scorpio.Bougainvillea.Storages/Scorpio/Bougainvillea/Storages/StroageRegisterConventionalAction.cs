using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Conventional;

namespace Scorpio.Bougainvillea.Storages
{
    internal class StroageRegisterConventionalAction : ConventionalActionBase
    {
        public StroageRegisterConventionalAction(IConventionalConfiguration configuration) : base(configuration)
        {
        }

        protected override void Action(IConventionalContext context)
        {
            context.Types.ForEach(t =>
            {
                var name = t.GetAttribute<StroageNameAttribute>().StorageName;
                context.Services.AddGrainStorage(t, name);
            });
        }
    }
}
