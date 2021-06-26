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
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        public readonly OrganizerContext organizerContext;
        //protected readonly OrganizerContext organizerContext;

        public BaseRepository(OrganizerContext oContext)
        {
            organizerContext = oContext ?? throw new ArgumentNullException(nameof(oContext));
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                await organizerContext.Set<T>().AddAsync(entity);

                await organizerContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Delete(T entity)
        {
            try
            {
                // organizerContext.Entry(entity).State = EntityState.Deleted;
                organizerContext.Set<T>().Remove(entity);

                await organizerContext.SaveChangesAsync();

                return;
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
                organizerContext.Set<T>().Remove(await organizerContext.Set<T>().FindAsync(id));

                await organizerContext.SaveChangesAsync();

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                return await organizerContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<T>> GetList()
        {
            try
            {
                return await organizerContext.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Update(T entity)
        {
            try
            {
                // organizerContext.Entry(entity).State = EntityState.Modified;
                organizerContext.Update(entity);

                await organizerContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
