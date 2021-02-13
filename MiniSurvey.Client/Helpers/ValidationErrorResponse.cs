using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class ValidationErrorResponse
    {
        public List<ValidationModel> Errors { get; set; }
    }
}
