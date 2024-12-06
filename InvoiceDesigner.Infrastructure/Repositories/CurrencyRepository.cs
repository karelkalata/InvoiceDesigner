using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class CurrencyRepository : ICurrencyRepository
	{
		private readonly DataContext _context;

		public CurrencyRepository(DataContext context)
		{
			_context = context;
		}


		public async Task<IReadOnlyCollection<Currency>> GetAllAsync()
		{
			return await _context.Currencies
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<IReadOnlyCollection<Currency>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<Currency>, IOrderedQueryable<Currency>> orderBy)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<Currency> query = _context.Currencies.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				var searchString = queryPaged.SearchString.ToLower();
				query = query.Where(c =>
					c.Name.ToLower().Contains(searchString) ||
					c.Description.ToLower().Contains(searchString));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}


		public async Task<int> CreateAsync(Currency entity)
		{
			await _context.Currencies.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<Currency?> GetByIdAsync(int id)
		{
			return await _context.Currencies
				.Where(a => a.Id == id)
				.FirstOrDefaultAsync();
		}


		public async Task<int> UpdateAsync(Currency entity)
		{
			_context.Currencies.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<bool> DeleteAsync(Currency entity)
		{
			_context.Currencies.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}


		public async Task<int> GetCountAsync(bool showDeleted = false)
		{
			IQueryable<Currency> query = _context.Currencies;

			if (!showDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			return await query.CountAsync();
		}

	}
}
