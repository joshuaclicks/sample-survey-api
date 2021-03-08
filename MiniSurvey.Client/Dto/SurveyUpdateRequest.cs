using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class SurveyUpdateRequest: SurveyRequest
    {
        [Required(ErrorMessage = "SurveyId is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid SurveyId supplied!")]
        public int Id { get; set; }
    }
}
