using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class SurveyQuestion
    {
        public long Id { get; set; }
        public long? SurveyId { get; set; }
        public long? QuestionId { get; set; }
        public DateTime? DateAdded { get; set; }

        public virtual Question Question { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
