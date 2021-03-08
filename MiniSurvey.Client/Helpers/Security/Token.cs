using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers.Security
{
    public class Token
    {
        public string AuthToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
