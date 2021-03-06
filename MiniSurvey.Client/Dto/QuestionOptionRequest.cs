﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class QuestionOptionRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "You have not selected a question to respond to.")]
        public int QuestionId { get; set; }

        public int OptionId { get; set; }

        public string TextAnswer { get; set; }
    }
}
