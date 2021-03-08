using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class OpinionPoll
    {
        public long OptionId { get; set; }
        public string Option { get; set; }
        public double Percentage { get; set; }
    }
}
