using OrganizerAPI.Models.Common;
using OrganizerAPI.Models.Models;
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
        public Role Role { get; set; }
        public List<UserTaskDTO> UserTasks { get; set; }

        public UserDTO()
        {
            UserTasks = new List<UserTaskDTO>();
        }
    }
}
