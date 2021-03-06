﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MiniSurvey.Client.Models
{
    public partial class User
    {
        public User()
        {
            UserResponses = new HashSet<UserResponse>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime DateRegistered { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<UserResponse> UserResponses { get; set; }
    }
}
