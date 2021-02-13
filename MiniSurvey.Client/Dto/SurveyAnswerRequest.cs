using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyAnswerRequest
    {
        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Invalid Email is Supplied!")]
        public string Email { get; set; }

        public List<QuestionOptionRequest> Responses { get; set; }
    }
}
