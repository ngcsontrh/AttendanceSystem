using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Specification
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDescending { get; }
        int? Take { get; }
        int? Skip { get; }
        bool IsPagingEnabled { get; }
    }

    public abstract class SpecificationBase<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Criteria { get; private set; } = x => true;
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
        public int? Take { get; private set; }
        public int? Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;

        public SpecificationBase(
            Expression<Func<TEntity, bool>>? criteria = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            Expression<Func<TEntity, object>>? orderByDescending = null,
            int? skip = null,
            int? take = null)
        {
            if (criteria != null)
            {
                AddCriteria(criteria);
            }
            if (orderBy != null)
            {
                AddOrderBy(orderBy);
            }
            if (orderByDescending != null)
            {
                AddOrderByDescending(orderByDescending);
            }
            if (skip.HasValue && take.HasValue)
            {
                ApplyPaging(skip.Value, take.Value);
            }
        }

        private void AddCriteria(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        private void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        private void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        private void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
