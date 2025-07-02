using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Specification;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class GenericReposiory<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly Education_System _dbContext;

        public GenericReposiory(Education_System dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public async Task<IQueryable<T>> GetAllWithSpec(ISpecification<T> specification = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

            if (specification != null)
            {
                query = specification.Apply(query);
            }

            return await Task.FromResult(query);
        }
        
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdWithSpecAsync(int id, ISpecification<T> specification = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

            if (specification != null)
            {
                query = specification.Apply(query);
            }

            return await query.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        

    }
}
