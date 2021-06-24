using OrganizerAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Shared.ModelsDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<RefreshToken> UserRefreshTokens { get; set; }

        public UserDTO()
        {
            UserRefreshTokens = new List<RefreshToken>();
        }
    }
}
