using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential.KidSystem
{
    internal class KidSubSystem : ISubSystem
    {
        public ValueTask InitializeAsync() => throw new NotImplementedException();
        public ValueTask OnSetupAsync(IAvatarBase avatarBase) => throw new NotImplementedException();
    }
}
