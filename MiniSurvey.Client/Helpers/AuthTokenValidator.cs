using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class AuthTokenValidator
    {
        public string AuthTokenHash { get; set; }
        public string AuthTokenSalt { get; set; }
        public string RefreshTokenSalt { get; set; }
        public string RefreshTokenHash { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
