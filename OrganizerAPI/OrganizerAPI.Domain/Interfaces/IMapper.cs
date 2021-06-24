using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IMapper
    {
        UserTaskDTO MapUserTask(UserTask value);
        UserTask MapUserTask(UserTaskDTO value);

        UserDTO MapUser(User value);
        User MapUser(UserDTO value);
        User MapUser(UserRequestDTO value);
    }
}
