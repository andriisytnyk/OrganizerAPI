using FluentValidation;
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
                entity.UserId = userId.Value;
                var validationResult = validator.Validate(entity);
                if (validationResult.IsValid)
                {
                    var result = mapper.MapUserTask(await repository.Create(mapper.MapUserTask(entity)));
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
                await repository.Delete(mapper.MapUserTask(entity));
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
                foreach (var item in (await repository.GetList()).Where(ut => ut.UserId == userId).Select(ut => ut))
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
                return mapper.MapUserTask(await repository.GetById(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTaskDTO> Update(UserTaskDTO entity, int? userId = null)
        {
            var validationResult = validator.Validate(entity);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            try
            {
                return mapper.MapUserTask(
                    await repository.Update(
                        mapper.MapUserTask(entity)
                        )
                    );
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
