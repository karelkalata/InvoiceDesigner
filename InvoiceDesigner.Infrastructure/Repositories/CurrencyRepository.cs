using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
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

		public async Task<IReadOnlyCollection<Currency>> GetAllCurrenciesAsync()
		{
			return await _context.Currencies
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<IReadOnlyCollection<Currency>> GetCurrenciesAsync(int pageSize, int page, string searchString, Func<IQueryable<Currency>, IOrderedQueryable<Currency>> orderBy)
		{

			int skip = (page - 1) * pageSize;

			IQueryable<Currency> query = _context.Currencies.AsNoTracking();

			if (!string.IsNullOrEmpty(searchString))
			{
				query = query.Where(c =>
					c.Name.ToLower().Contains(searchString.ToLower()) ||
					c.Description.ToLower().Contains(searchString.ToLower()));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();
		}


		public async Task<int> CreateCurrencyAsync(Currency entity)
		{
			await _context.Currencies.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<Currency?> GetCurrencyByIdAsync(int id)
		{
			return await _context.Currencies
				.Where(a => a.Id == id)
				.FirstOrDefaultAsync();
		}


		public async Task<int> UpdateCurrencyAsync(Currency entity)
		{
			_context.Currencies.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}


		public async Task<bool> DeleteCurrencyAsync(Currency entity)
		{
			_context.Currencies.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}


		public async Task<int> GetCountCurrenciesAsync()
		{
			return await _context.Currencies.CountAsync();
		}

	}
}
