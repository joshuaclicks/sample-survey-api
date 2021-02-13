using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class QuestionResponse : DefaultResponse
    {
        public List<DefaultResponse> Options { get; set; }
    }
}
