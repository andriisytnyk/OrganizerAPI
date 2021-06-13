using OrganizerAPI.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IService<T> where T : Entity
    {
        Task<List<T>> GetAll();

        Task<T> GetById(int id);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task Delete(T entity);

        Task DeleteById(int id);
    }
}
