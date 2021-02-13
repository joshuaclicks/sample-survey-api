using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Helpers
{
    public class SuccessResponse<T> : ResponseValidationModel where T : class
    {
        public T Data { get; set; }
    }
}
