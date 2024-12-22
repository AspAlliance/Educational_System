using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Specification
{
    public interface ISpecification<T>
    {
        IQueryable<T> Apply(IQueryable<T> query);

        Expression<Func<T, bool>> Criteria { get; }

        Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; }
        Func<IQueryable<T>, IOrderedQueryable<T>> OrderByDescending { get; }

        int? Skip { get; }
        int? Take { get; }
    }
}
