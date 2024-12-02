using EducationalSystem.BLL.Specification;
using EducationalSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IQueryable<T>> GetAll();

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<IQueryable<T>> GetAllWithSpec(ISpecification<T> specification = null);

        Task<T> GetByIdWithSpecAsync(int id, ISpecification<T> specification = null);
    }
}
