﻿using FluentValidation;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Infrastructure.Repositories;
using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly UserTaskRepository repository;
        private readonly IMapper mapper;
        private readonly AbstractValidator<UserTaskDTO> validator;

        public UserTaskService(
            UserTaskRepository taskRepository,
            IMapper mapper,
            AbstractValidator<UserTaskDTO> userTaskValidator
            )
        {
            this.repository = taskRepository;
            this.mapper = mapper;
            this.validator = userTaskValidator;
        }

        public async Task<UserTaskDTO> Create(UserTaskDTO entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                var validationResult = validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    var userTask = mapper.MapUserTask(entity);
                    userTask.UserId = userId.Value;
                    var result = mapper.MapUserTask(await repository.Create(userTask));
                    return result;
                }
                else
                    throw new ValidationException(validationResult.Errors);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(UserTaskDTO entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                var userTask = mapper.MapUserTask(entity);
                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");
                await repository.Delete(userTask);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteById(int id, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                var userTask = await repository.GetById(id);
                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");
                await repository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserTaskDTO>> GetAll(int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                var result = new List<UserTaskDTO>();
                foreach (var item in (await repository.GetList()).Where(ut => ut.UserId == userId))
                {
                    result.Add(mapper.MapUserTask(item));
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTaskDTO> GetById(int id, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");
                var userTask = await repository.GetById(id);
                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");
                return mapper.MapUserTask(userTask);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTaskDTO> Update(UserTaskDTO entity, int? userId = null)
        {
            if (userId == null)
                throw new Exception("UserId was not included.");
            var validationResult = validator.Validate(entity);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            try
            {
                var userTask = await repository.GetById(entity.Id);
                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");
                //var result = mapper.MapUserTask(entity);
                userTask.Title = entity.Title;
                userTask.Description = entity.Description;
                userTask.Status = entity.Status;
                //result.UserId = userId.Value;
                return mapper.MapUserTask(await repository.Update(userTask));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
