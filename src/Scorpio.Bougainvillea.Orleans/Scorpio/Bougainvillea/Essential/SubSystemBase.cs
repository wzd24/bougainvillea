using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scorpio.Bougainvillea.Essential
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SubSystemBase : ISubSystem
    {
        ValueTask ISubSystem.InitializeAsync() => throw new NotImplementedException();
        ValueTask ISubSystem.OnSetupAsync(IAvatarBase avatarBase)
        {
            throw new NotImplementedException();
        }
    }
}
