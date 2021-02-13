using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyStat
    {
        public int TotalParticipants { get; set; }
        public int TotalQuestions { get; set; }
        public List<Poll> Polls { get; set; }
    }
}
