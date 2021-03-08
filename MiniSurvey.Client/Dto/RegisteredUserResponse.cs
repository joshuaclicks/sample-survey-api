using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class RegisteredUserResponse
    {
        public UserResponse User { get; set; }
        public DefaultResponse Role { get; set; }
    }
}
