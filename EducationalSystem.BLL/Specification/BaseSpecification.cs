using System;
using System.Linq;
using System.Linq.Expressions;

namespace EducationalSystem.BLL.Specification
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; private set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderByDescending { get; private set; }
        public int? Skip { get; private set; }
        public int? Take { get; private set; }

        protected BaseSpecification(Expression<Func<T, bool>> criteria = null)
        {
            Criteria = criteria;
        }

        public void AddOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddOrderByDescending(Func<IQueryable<T>, IOrderedQueryable<T>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        public void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public virtual IQueryable<T> Apply(IQueryable<T> query)
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }

            if (OrderBy != null)
            {
                query = OrderBy(query);
            }
            else if (OrderByDescending != null)
            {
                query = OrderByDescending(query);
            }

            if (Skip.HasValue)
            {
                query = query.Skip(Skip.Value);
            }

            if (Take.HasValue)
            {
                query = query.Take(Take.Value);
            }

            return query;
        }
    }
}
