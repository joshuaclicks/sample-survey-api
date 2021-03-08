using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsOptionRequired { get; set; }
        public int? MinimumRequired { get; set; }
        public int? MaximumRequired { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
