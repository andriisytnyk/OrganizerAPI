using OrganizerAPI.Models.Common;
using System.Collections.Generic;

namespace OrganizerAPI.Shared.ModelsDTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public List<UserTaskDto> UserTasks { get; set; }

        public UserDto()
        {
            UserTasks = new List<UserTaskDto>();
        }
    }
}
