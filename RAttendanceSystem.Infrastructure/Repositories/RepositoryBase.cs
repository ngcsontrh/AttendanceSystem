using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly RAttendanceDbContext _context;

        protected RepositoryBase(RAttendanceDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var exists = await _context.Set<TEntity>().AnyAsync(predicate);
            return exists;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = await _context.Set<TEntity>().CountAsync(predicate);
            return count;
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IReadOnlyList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var records = await _context.Set<TEntity>().Where(predicate).ToListAsync();
            return records;
        }

        public async Task<(IReadOnlyList<TEntity>, int)> GetPageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, SortDirection sortDirection = SortDirection.Asc, int limit = 0, int offset = 10)
        {
            var queryable = _context.Set<TEntity>().Where(predicate);
            var totalRecords = queryable.Count();
            if (sortDirection == SortDirection.Asc)
            {
                queryable = queryable.OrderBy(orderBy);
            }
            else
            {
                queryable = queryable.OrderByDescending(orderBy);
            }
            var records = await queryable.Skip(offset).Take(limit).ToListAsync();
            return (records, totalRecords);
        }

        public async Task<(IReadOnlyList<TEntity>, int)> GetPageAsync(ISpecification<TEntity> specification)
        {
            var predicate = specification.Criteria;
            var queryable = _context.Set<TEntity>().Where(predicate);
            var totalRecords = await queryable.CountAsync();

            if (specification.OrderBy != null)
            {
                queryable = queryable.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                queryable = queryable.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPagingEnabled && specification.Skip.HasValue && specification.Take.HasValue)
            {
                queryable = queryable.Skip(specification.Skip.Value).Take(specification.Take.Value);
            }
            var records = await queryable.ToListAsync();
            return (records, totalRecords);
        }

        public IQueryable<TEntity> GetQueryable() => _context.Set<TEntity>();

        public async Task<TEntity?> GetRecordAsync(Guid id)
        {
            var record = await _context.Set<TEntity>().FindAsync(id);
            return record;
        }

        public Task<TEntity?> GetRecordAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var record = _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return record;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }        
    }
}
