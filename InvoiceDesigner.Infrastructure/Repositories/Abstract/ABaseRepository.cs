using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Abstract
{
	public abstract class ABaseRepository<T> : IABaseRepository<T> where T : class, IABaseEntity
	{
		protected readonly DataContext _context;
		protected readonly DbSet<T> _dbSet;

		protected ABaseRepository(DataContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public virtual async Task<IReadOnlyCollection<T>> GetEntitiesAsync(PagedFilter pagedFilter)
		{
			int skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

			IQueryable<T> query = _dbSet.AsNoTracking();

			if (!pagedFilter.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(pagedFilter.SearchString))
			{
				var searchString = pagedFilter.SearchString.ToLower();
				query = query.Where(c => c.Name.ToLower().Contains(searchString));
			}

			if (!string.IsNullOrEmpty(pagedFilter.ExcludeString))
			{
				var excludeString = pagedFilter.ExcludeString.ToLower();
				query = query.Where(c => !c.Name.ToLower().Contains(excludeString));
			}

			if (!string.IsNullOrEmpty(pagedFilter.SortLabel))
			{
				query = GetOrdering(pagedFilter.SortLabel)(query);
			}

			return await query
				.Skip(skip)
				.Take(pagedFilter.PageSize)
				.ToListAsync();
		}

		public virtual async Task CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task<T?> GetByIdAsync(GetByIdFilter getByIdFilter)
		{
			return await _dbSet
				.Where(a => a.Id == getByIdFilter.Id)
				.FirstOrDefaultAsync();
		}

		public virtual async Task<bool> UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public virtual async Task<bool> DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public virtual async Task<int> GetCountAsync(GetCountFilter _getCountFilter)
		{
			IQueryable<T> query = _dbSet;

			if (!_getCountFilter.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!_getCountFilter.ShowArchived)
			{
				query = query.Where(e => e.IsArchived == false);
			}

			return await query.CountAsync();
		}

		public virtual Func<IQueryable<T>, IOrderedQueryable<T>> GetOrdering(string sortLabel)
		{
			var sortingOptions = new Dictionary<string, Func<IQueryable<T>, IOrderedQueryable<T>>>
			{
				{ "Id_desc", q => q.OrderByDescending(e => e.Id) },
				{ "Name", q => q.OrderBy(e => e.Name) },
				{ "Name_desc", q => q.OrderByDescending(e => e.Name) }
			};

			return sortingOptions.TryGetValue(sortLabel, out var orderFunc) ? orderFunc : q => q.OrderBy(e => e.Id);
		}
	}
}
