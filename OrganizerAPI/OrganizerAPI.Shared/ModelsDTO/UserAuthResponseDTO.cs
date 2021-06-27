using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrganizerAPI.Shared.ModelsDTO
{
    public class UserAuthResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<UserTaskDto> UserTasks { get; set; }
        public string JwtToken { get; set; }
        /// <summary>
        /// Gets or sets token expiration time in seconds.
        /// </summary>
        public int ExpiresIn { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public UserAuthResponseDto(UserDto userDto, string jwtToken, int expiresIn, string refreshToken)
        {
            Id = userDto.Id;
            FirstName = userDto.FirstName;
            LastName = userDto.LastName;
            Username = userDto.Username;
            Email = userDto.Email;
            UserTasks = userDto.UserTasks;
            JwtToken = jwtToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }
    }
}
