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
        private readonly UserTaskRepository _repository;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<UserTaskDto> _validator;

        public UserTaskService(
            UserTaskRepository taskRepository,
            IMapper mapper,
            AbstractValidator<UserTaskDto> userTaskValidator
            )
        {
            _repository = taskRepository;
            _mapper = mapper;
            _validator = userTaskValidator;
        }

        public async Task<UserTaskDto> Create(UserTaskDto entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");

                var validationResult = await _validator.ValidateAsync(entity);
                if (!validationResult.IsValid) 
                    throw new ValidationException(validationResult.Errors);

                var userTask = _mapper.MapUserTask(entity);
                userTask.UserId = userId.Value;
                var result = _mapper.MapUserTask(await _repository.Create(userTask));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(UserTaskDto entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");

                var userTask = await _repository.GetById(entity.Id);

                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");

                await _repository.Delete(userTask);
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

                var userTask = await _repository.GetById(id);

                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");

                await _repository.DeleteById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserTaskDto>> GetAll(int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");

                return (await _repository.GetList()).Where(ut => ut.UserId == userId).Select(item => _mapper.MapUserTask(item)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTaskDto> GetById(int id, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");

                var userTask = await _repository.GetById(id);

                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");

                return _mapper.MapUserTask(userTask);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTaskDto> Update(UserTaskDto entity, int? userId = null)
        {
            try
            {
                if (userId == null)
                    throw new Exception("UserId was not included.");

                var validationResult = await _validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);
            
                var userTask = await _repository.GetById(entity.Id);

                if (userTask.UserId != userId)
                    throw new Exception("User isn't this task owner.");

                var result = _mapper.MapUserTask(entity);
                result.UserId = userId.Value;

                return _mapper.MapUserTask(await _repository.Update(result));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
