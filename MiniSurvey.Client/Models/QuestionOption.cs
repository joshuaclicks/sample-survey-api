using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class QuestionOption
    {
        public int Id { get; set; }
        public long QuestionId { get; set; }
        public long OptionId { get; set; }

        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
    }
}
