using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly RAttendanceDbContext _context;

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

        public async Task<(IReadOnlyList<TEntity>, int)> GetPageAsync(IQueryable<TEntity> queryable, Expression<Func<TEntity, object>> orderBy, SortDirection sortDirection = SortDirection.Asc, int limit = 0, int offset = 10)
        {
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

        public Task<(IReadOnlyList<TEntity>, int)> GetPageAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> orderBy, SortDirection sortDirection = SortDirection.Asc, int limit = 0, int offset = 10)
        {
            var queryable = _context.Set<TEntity>().Where(predicate);
            return GetPageAsync(queryable, orderBy, sortDirection, limit, offset);
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
