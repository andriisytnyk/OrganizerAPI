﻿using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Mapping
{
    public class Mapper : IMapper
    {
        public UserTaskDTO MapUserTask(UserTask value)
        {
            return new UserTaskDTO
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                Date = value.Date,
                Status = value.Status,
                UserId = value.UserId
            };
        }

        public UserTask MapUserTask(UserTaskDTO value)
        {
            return new UserTask
            {
                Id = value.Id,
                Title = value.Title,
                Description = value.Description,
                Date = value.Date,
                Status = value.Status,
                UserId = value.UserId
            };
        }

        public UserDTO MapUser(User value)
        {
            return new UserDTO
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Username = value.Username
            };
        }

        // ???
        public User MapUser(UserDTO value)
        {
            throw new NotImplementedException();
        }
    }
}
