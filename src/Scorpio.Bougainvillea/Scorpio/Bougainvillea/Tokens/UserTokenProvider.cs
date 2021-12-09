using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scorpio.Bougainvillea;
using Scorpio.Bougainvillea.Tokens;
using Scorpio.DependencyInjection;

namespace Sailina.Tang.Tokens
{
    internal class UserTokenProvider : IUserTokenProvider, ISingletonDependency
    {
        private readonly ITokenEncodeProvider _tokenEncodeProvider;

        public UserTokenProvider(ITokenEncodeProvider tokenEncodeProvider)
        {
            _tokenEncodeProvider = tokenEncodeProvider;
        }
        public string GenerateToken(object user)
        {
            return _tokenEncodeProvider.Encode(user);
        }

        public T GetUserData<T>(string token)
        {
            return _tokenEncodeProvider.Decode<T>(token);
        }

    }
}
