using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSurvey.Client.Dto
{
    public class UserRequest
    {
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Invalid Email is Supplied!")]
        public string Email { get; set; }
    }
}
