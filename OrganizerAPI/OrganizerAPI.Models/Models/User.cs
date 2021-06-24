﻿using OrganizerAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrganizerAPI.Models.Models
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; } = Role.User;

        [JsonIgnore]
        public List<RefreshToken> UserRefreshTokens { get; set; }

        public List<UserTask> UserTasks { get; set; }

        public User()
        {
            UserRefreshTokens = new List<RefreshToken>();
            UserTasks = new List<UserTask>();
        }
    }
}
