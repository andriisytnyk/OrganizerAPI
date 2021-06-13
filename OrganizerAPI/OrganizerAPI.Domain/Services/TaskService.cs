using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskRepository repository;

        public TaskService(TaskRepository taskRepository)
        {

        }

        public int Create(Models.Models.Task entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Models.Models.Task entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Models.Models.Task> GetAll()
        {
            throw new NotImplementedException();
        }

        public Models.Models.Task GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Models.Models.Task entity)
        {
            throw new NotImplementedException();
        }
    }
}
