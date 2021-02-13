using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class QuestionOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }

        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
    }
}
