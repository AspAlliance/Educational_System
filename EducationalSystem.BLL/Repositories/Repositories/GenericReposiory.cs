using EducationalSystem.BLL.Repositories.Interfaces;
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
            // if you want to Check if the generic type T is of specific type use this (check Example)↓↓↓
            // Use AsNoTracking to optimize for read-only queries (improves performance).
            #region Example
            /*if (typeof(T) == typeof(Courses))
            {
                return (IQueryable<T>)_dbContext.Set<Courses>().Include(C => C.Lessons)
                                                               .Include(C => C.Course_Instructors).AsNoTracking().OrderByDescending(C => C.CourseID).Take(1000);
            }
            else*/
            #endregion

            return (IQueryable<T>)_dbContext.Set<T>().AsNoTracking(); // Optimize for read-only query
        }

        public async Task<T> GetByIdAsync(int id)
        {
            // if you want to Check if the generic type T is of specific type use this (check Example)↓↓↓
            #region Example
            /*if (typeof(T) == typeof(Courses))
            {
                var course = await _dbContext.Set<Courses>().Include(C => C.Lessons)
                                                               .Include(C => C.Course_Instructors).FirstOrDefaultAsync(a => a.CourseID == id);
                return course as T;
            }
            else*/
            #endregion

            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
