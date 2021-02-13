using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class ErrorResponse<T> : ResponseValidationModel
    {
        public T Data { get; set; }
    }
}
