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
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly OrganizerContext organizerContext;

        public BaseRepository(OrganizerContext oContext)
        {
            organizerContext = oContext ?? throw new ArgumentNullException(nameof(oContext));
        }

        public async Task<T> Create(T entity)
        {
            await organizerContext.Set<T>().AddAsync(entity);

            await organizerContext.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(T entity)
        {
            // organizerContext.Entry(entity).State = EntityState.Deleted;
            organizerContext.Set<T>().Remove(entity);

            await organizerContext.SaveChangesAsync();

            return;
        }

        public async Task DeleteById(int id)
        {
            organizerContext.Set<T>().Remove(await organizerContext.Set<T>().FindAsync(id));

            await organizerContext.SaveChangesAsync();

            return;
        }

        public async Task<T> GetById(int id)
        {
            return await organizerContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetList()
        {
            return await organizerContext.Set<T>().ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            // organizerContext.Entry(entity).State = EntityState.Modified;
            organizerContext.Update(entity);

            await organizerContext.SaveChangesAsync();

            return entity;
        }
    }
}
