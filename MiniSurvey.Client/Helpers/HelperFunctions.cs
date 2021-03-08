using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public static class HelperFunctions
    {
        public static string GetRole(int id)
        {
            if (id == (int)Enums.Roles.User)
                return Enums.Roles.User.ToString();

            else if (id == (int)Enums.Roles.Administrator)
                return Enums.Roles.Administrator.ToString();

            else
                return null;
        }
    }
}
