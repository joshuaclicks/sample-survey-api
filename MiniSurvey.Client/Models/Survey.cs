using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class Survey
    {
        public Survey()
        {
            SurveyQuestions = new HashSet<SurveyQuestion>();
            UserResponses = new HashSet<UserResponse>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
