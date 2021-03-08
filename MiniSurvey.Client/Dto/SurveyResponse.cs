using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyResponse: SurveyDetail
    {
        public List<QuestionResponse> SurveyQuestions { get; set; }
    }
}
