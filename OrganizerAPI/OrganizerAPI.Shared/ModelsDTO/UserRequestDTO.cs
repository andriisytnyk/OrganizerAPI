using OrganizerAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Shared.ModelsDTO
{
    public class UserRequestDTO : UserDTO
    {
        public string Password { get; set; }
        public List<RefreshToken> UserRefreshTokens { get; set; }

        public UserRequestDTO() : base()
        {
            UserRefreshTokens = new List<RefreshToken>();
        }
    }
}
