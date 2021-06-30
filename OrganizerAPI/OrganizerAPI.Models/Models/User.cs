using OrganizerAPI.Models.Common;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
