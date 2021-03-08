using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers.Security
{
    public class JwtClaimIdentifiers
    {
        public const string Role = "role", UserId = "id";
        public const string ApiAccess = "api_access";
    }
}
