using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class QuestionTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOptionRequired { get; set; }
        public int? MinimumRequired { get; set; }
        public int? MaximumRequired { get; set; }
    }
}
