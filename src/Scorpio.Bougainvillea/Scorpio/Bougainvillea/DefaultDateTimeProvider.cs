using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.DependencyInjection;

namespace Scorpio.Bougainvillea
{
    internal class DefaultDateTimeProvider : IDateTimeProvider,ISingletonDependency
    {

        public DefaultDateTimeProvider()
        {
        }

        public ValueTask<DateTimeOffset> GetNowAsync() => new ValueTask<DateTimeOffset>(DateTimeOffset.Now);
    }
}
