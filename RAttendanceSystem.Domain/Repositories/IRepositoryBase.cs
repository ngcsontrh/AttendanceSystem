using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetQueryable();
        Task<TEntity?> GetRecordAsync(Guid id);
        Task<TEntity?> GetRecordAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<(IReadOnlyList<TEntity>, int)> GetPageAsync(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> orderBy,
            SortDirection sortDirection = SortDirection.Asc,
            int limit = 0,
            int offset = 10);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<(IReadOnlyList<TEntity>, int)> GetPageAsync(ISpecification<TEntity> specification);
        Task<int> SaveChangesAsync();
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}
