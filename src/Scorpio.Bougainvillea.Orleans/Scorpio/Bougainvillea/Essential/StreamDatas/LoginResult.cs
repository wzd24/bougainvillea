using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea.Essential;

namespace Sailina.Tang.Essential.StreamDatas
{
    internal class LoginStatusNotify
    {
        public long AvatarId { get; set; }

        public AvatarInfoStatus  Status { get; set; }
    }
}
