using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<List<T>> GetAll(int? userId = null);

        Task<T> GetById(int id, int? userId = null);

        Task<T> Create(T entity, int? userId = null);

        Task<T> Update(T entity, int? userId = null);

        Task Delete(T entity, int? userId = null);

        Task DeleteById(int id, int? userId = null);
    }
}
