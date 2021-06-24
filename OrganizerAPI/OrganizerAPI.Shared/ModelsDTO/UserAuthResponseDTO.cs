using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrganizerAPI.Shared.ModelsDTO
{
    public class UserAuthResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<UserTaskDTO> UserTasks { get; set; }
        public string JwtToken { get; set; }
        /// <summary>
        /// Gets or sets token expiration time in seconds.
        /// </summary>
        public int ExpiresIn { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public UserAuthResponseDTO(UserDTO userDTO, string jwtToken, int expiresIn, string refreshToken)
        {
            Id = userDTO.Id;
            FirstName = userDTO.FirstName;
            LastName = userDTO.LastName;
            Username = userDTO.Username;
            Email = userDTO.Email;
            UserTasks = userDTO.UserTasks;
            JwtToken = jwtToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }
    }
}
