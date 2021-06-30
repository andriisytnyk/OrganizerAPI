using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared.ModelsDTO;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IMapper
    {
        UserTaskDto MapUserTask(UserTask value);
        UserTask MapUserTask(UserTaskDto value);

        UserDto MapUser(User value);
        User MapUser(UserDto value);
        User MapUser(UserRequestDto value);
    }
}
