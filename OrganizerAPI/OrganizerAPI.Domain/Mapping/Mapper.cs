using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared.ModelsDTO;
using System.Collections.Generic;

namespace OrganizerAPI.Domain.Mapping
{
    public class Mapper : IMapper
    {
        public UserTaskDto MapUserTask(UserTask value)
        {
            return new UserTaskDto
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                Date = value.Date,
                Status = value.Status
            };
        }

        public UserTask MapUserTask(UserTaskDto value)
        {
            return new UserTask
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                Date = value.Date,
                Status = value.Status
            };
        }

        public UserDto MapUser(User value)
        {
            var userTasks = new List<UserTaskDto>();
            foreach (var item in value.UserTasks)
            {
                userTasks.Add(MapUserTask(item));
            }
            return new UserDto
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Username = value.Username,
                Email = value.Email,
                Role = value.Role,
                UserTasks = userTasks
            };
        }

        public User MapUser(UserDto value)
        {
            var userTasks = new List<UserTask>();
            foreach (var item in value.UserTasks)
            {
                userTasks.Add(MapUserTask(item));
            }
            return new User
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Username = value.Username,
                Email = value.Email,
                Role = value.Role,
                UserTasks = userTasks
            };
        }

        public User MapUser(UserRequestDto value)
        {
            var userTasks = new List<UserTask>();
            foreach (var item in value.UserTasks)
            {
                userTasks.Add(MapUserTask(item));
            }
            return new User
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Username = value.Username,
                Password = value.Password,
                Email = value.Email,
                Role = value.Role,
                UserTasks = userTasks,
                UserRefreshTokens = value.UserRefreshTokens
            };
        }
    }
}
