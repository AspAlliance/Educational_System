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

            if (typeof(T) == typeof(Instructors))
            {
                return (IQueryable<T>)_dbContext.Set<Instructors>().Include(i => i.applicationUser)
                                                            .Include(i => i.Course_Instructors)
                                                            .ThenInclude(ci => ci.Courses)
                                                            .Include(i => i.Specializations);
                                                            
            }
            //else if (typeof(T) == typeof(Assessments))
            //{
            //    return (IQueryable<T>)_dbContext.Set<Assessments>().Include(i => i.Courses)
            //                                                       .Include(l => l.Lessons)
            //                                                       .Include(ts => ts.TextSubmissions)
            //                                                       .Include(fs => fs.FileSubmissions)
            //                                                       .Include(r => r.Rubrics)
            //                                                       .AsNoTracking();
            //}
            else
            return (IQueryable<T>)_dbContext.Set<T>().AsNoTracking(); // Optimize for read-only query
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Instructors))
            {
                var course = await _dbContext.Set<Instructors>().Include(i => i.Course_Instructors)
                                                            .ThenInclude(ci => ci.Courses)
                                                            .Include(i => i.Specializations).FirstOrDefaultAsync(a => a.ID == id);
                return course as T;
            }
            else
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
