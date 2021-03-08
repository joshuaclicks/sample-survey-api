using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class UserResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public long SurveyId { get; set; }
        public long QuestionId { get; set; }
        public long? OptionId { get; set; }
        public string TextResponse { get; set; }
        public DateTime DateResponded { get; set; }

        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual User User { get; set; }
    }
}
