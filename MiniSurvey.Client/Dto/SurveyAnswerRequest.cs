using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyAnswerRequest
    {
        public int SurveyId { get; set; }
        public List<QuestionOptionRequest> Responses { get; set; }
    }
}
