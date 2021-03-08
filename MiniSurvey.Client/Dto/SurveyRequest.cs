using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyRequest
    {
        [Required(ErrorMessage = "Title is Required!")]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
