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
            SurveyQuestions = new HashSet<SurveyQuestion>();
            UserResponses = new HashSet<UserResponse>();
        }

        public long Id { get; set; }
        public string Text { get; set; }
        public int QuestionTypeId { get; set; }

        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
