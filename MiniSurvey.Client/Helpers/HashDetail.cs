using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class HashDetail
    {
        public string Salt { get; set; }
        public string HashedValue { get; set; }
    }
}
