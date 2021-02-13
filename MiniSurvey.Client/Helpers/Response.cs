using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class Response<T> where T : class
    {
        public SuccessResponse<T> ResponseBody { get; set; }
        public ResponseValidationModel ErrorResponse { get; set; }
    }
}
