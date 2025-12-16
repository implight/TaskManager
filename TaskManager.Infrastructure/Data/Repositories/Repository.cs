using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Data.Repositories;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _set;

        protected Repository(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<T>(); 
        }

        protected abstract IQueryable<T> Include(IQueryable<T> set);

        public virtual Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Include(_set).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public virtual Task<T[]> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => Include(_set).Where(predicate).ToArrayAsync(cancellationToken);

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => Include(_set).AnyAsync(predicate, cancellationToken);

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => Include(_set).CountAsync(predicate, cancellationToken);

        public virtual async Task<PaginatedList<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, CancellationToken cancellationToken = default)
        {
            var query = Include(_set).Where(predicate);

            var totalCount = await query.CountAsync(cancellationToken);

            query = orderBy(query);

            var items = await query
                .Paginate(pageNumber, pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = items
            };
        }

        public virtual void Add(T entity)
            => _set.Add(entity);

        public virtual void Update(T entity)
        {
            if (_set.Entry(entity).State != EntityState.Modified)
            {
                _set.Update(entity);
            }
        }

        public virtual void Delete(T entity)
            => _set.Remove(entity);
    }
}
