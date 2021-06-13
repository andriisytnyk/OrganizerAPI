using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetById(int id);

        Task<List<T>> GetList();

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task Delete(T entity);

        Task DeleteById(int id);
    }
}
