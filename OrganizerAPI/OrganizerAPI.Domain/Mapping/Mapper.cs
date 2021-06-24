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
                Status = value.Status
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
                Status = value.Status
            };
        }

        public UserDTO MapUser(User value)
        {
            var userTasks = new List<UserTaskDTO>();
            foreach (var item in value.UserTasks)
            {
                userTasks.Add(MapUserTask(item));
            }
            return new UserDTO
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Username = value.Username,
                Email = value.Email,
                UserTasks = userTasks
            };
        }

        public User MapUser(UserDTO value)
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
                UserTasks = userTasks
            };
        }

        public User MapUser(UserRequestDTO value)
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
                UserTasks = userTasks,
                Password = value.Password,
                UserRefreshTokens = value.UserRefreshTokens,
                Role = value.Role
            };
        }
    }
}