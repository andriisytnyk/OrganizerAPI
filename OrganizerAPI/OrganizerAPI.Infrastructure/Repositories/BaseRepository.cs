using Microsoft.EntityFrameworkCore;
using OrganizerAPI.Infrastructure.Contexts;
using OrganizerAPI.Infrastructure.Interfaces;
using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrganizerAPI.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        public readonly OrganizerContext OrganizerContext;
        //protected readonly OrganizerContext organizerContext;

        protected BaseRepository(OrganizerContext oContext)
        {
            OrganizerContext = oContext ?? throw new ArgumentNullException(nameof(oContext));
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                await OrganizerContext.Set<T>().AddAsync(entity);

                await OrganizerContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(T entity)
        {
            try
            {
                // organizerContext.Entry(entity).State = EntityState.Deleted;
                OrganizerContext.Set<T>().Remove(entity);

                await OrganizerContext.SaveChangesAsync();

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
                OrganizerContext.Set<T>().Remove(await OrganizerContext.Set<T>().FindAsync(id));

                await OrganizerContext.SaveChangesAsync();

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
                return await OrganizerContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
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
                return await OrganizerContext.Set<T>().AsNoTracking().ToListAsync();
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
                OrganizerContext.Update(entity);

                await OrganizerContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
