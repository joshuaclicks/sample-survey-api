using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class StringFormatter
    {
        public static string PascalCaseConverter(string value)
        {
            string newValue = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                var characterArray = value.ToCharArray();
                for (int i = 0; i < characterArray.Length; i++)
                {
                    if (i == 0)
                    {
                        newValue += characterArray[i].ToString().ToLower();
                    }
                    else
                    {
                        newValue += characterArray[i];
                    }
                }
            }
            return newValue;
        }
    }
}
