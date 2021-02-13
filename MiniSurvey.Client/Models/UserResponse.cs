using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class UserResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public DateTime DateResponded { get; set; }

        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
