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

        Task<IReadOnlyList<T>> GetList();

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
