using MiniSurvey.Client.Helpers.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class ValueGenerator
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {

            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                authToken = await jwtFactory.GenerateEncodedToken(userName, identity),
                expiresIn = (int)jwtOptions.ValidFor.TotalSeconds,
                refreshToken = Guid.NewGuid().ToString()
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
