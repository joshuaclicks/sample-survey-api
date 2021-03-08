using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class Option
    {
        public Option()
        {
            QuestionOptions = new HashSet<QuestionOption>();
            UserResponses = new HashSet<UserResponse>();
        }

        public long Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
