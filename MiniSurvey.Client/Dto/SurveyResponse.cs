using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyResponse
    {
        public List<QuestionResponse> Questions { get; set; }
    }
}
