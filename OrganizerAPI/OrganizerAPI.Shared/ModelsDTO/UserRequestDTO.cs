using OrganizerAPI.Models.Common;
using System.Collections.Generic;

namespace OrganizerAPI.Shared.ModelsDTO
{
    public class UserRequestDto : UserDto
    {
        public string Password { get; set; }
        public List<RefreshToken> UserRefreshTokens { get; set; }

        public UserRequestDto() : base()
        {
            UserRefreshTokens = new List<RefreshToken>();
        }
    }
}
