using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class Poll
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<OpinionPoll> OpinionPolls { get; set; }
    }
}
