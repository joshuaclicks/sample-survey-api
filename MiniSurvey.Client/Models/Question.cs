using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class Question
    {
        public Question()
        {
            QuestionOptions = new HashSet<QuestionOption>();
            UserResponses = new HashSet<UserResponse>();
        }

        public int Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
