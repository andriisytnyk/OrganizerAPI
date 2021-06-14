using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Infrastructure.Repositories;
using OrganizerAPI.Models.Models;
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

        public UserTaskService(UserTaskRepository taskRepository)
        {
            repository = taskRepository;
        }

        public async Task<UserTask> Create(UserTask entity)
        {
            try
            {
                return await repository.Create(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(UserTask entity)
        {
            try
            {
                await repository.Delete(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteById(int id)
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

        public async Task<List<UserTask>> GetAll()
        {
            try
            {
                return await repository.GetList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTask> GetById(int id)
        {
            try
            {
                return await repository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserTask> Update(UserTask entity)
        {
            try
            {
                return await repository.Update(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
