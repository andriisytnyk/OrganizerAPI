using OrganizerAPI.Models.Models;
using System.Collections.Generic;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IService<T> where T : Entity
    {
        List<T> GetAll();

        T GetById(int id);

        int Create(T entity);

        void Update(int id, T entity);

        void Delete(T entity);

        void DeleteById(int id);
    }
}
