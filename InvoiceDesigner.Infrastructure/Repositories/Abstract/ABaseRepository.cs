using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Records;
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

		public virtual async Task<IReadOnlyCollection<T>> GetEntitiesAsync(QueryPaged queryPaged, string sortLabel)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<T> query = _dbSet.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				var searchString = queryPaged.SearchString.ToLower();
				query = query.Where(c => c.Name.ToLower().Contains(searchString));
			}

			query = GetOrdering(sortLabel)(query);

			return await query
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}

		public virtual async Task<int> CreateAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public virtual async Task<T?> GetByIdAsync(int id)
		{
			return await _dbSet
				.Where(a => a.Id == id)
				.FirstOrDefaultAsync();
		}

		public virtual async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			await _context.SaveChangesAsync();
		}

		public virtual async Task<bool> DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public virtual async Task<int> GetCountAsync(GetCountFilter recordGetCount)
		{
			IQueryable<T> query = _dbSet;

			if (!recordGetCount.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!recordGetCount.ShowArchived)
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
