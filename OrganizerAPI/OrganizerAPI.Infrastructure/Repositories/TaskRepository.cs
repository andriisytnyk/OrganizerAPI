using Microsoft.EntityFrameworkCore;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Infrastructure.Interfaces;
using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public class TaskRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly OrganizerContext organizerContext;

        public TaskRepository(OrganizerContext oContext)
        {
            organizerContext = oContext ?? throw new ArgumentNullException(nameof(oContext));
        }

        public void Add(T entity)
        {
            organizerContext.Set<T>().Add(entity);

            organizerContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            organizerContext.Entry(entity).State = EntityState.Deleted;

            organizerContext.SaveChanges();
        }

        public async Task<T> GetById(int id)
        {
            return await organizerContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetList()
        {
            return await organizerContext.Set<T>().ToListAsync();
        }

        public void Update(T entity)
        {
            organizerContext.Entry(entity).State = EntityState.Modified;

            organizerContext.SaveChanges();
        }
    }
}
