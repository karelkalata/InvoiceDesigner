using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class BankRepository : IBankRepository
	{
		private readonly DataContext _context;

		public BankRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<Bank>> GetAllBanksAsync()
		{
			return await _context.Banks
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<IReadOnlyCollection<Bank>> GetBanksAsync(int pageSize, int page, string searchString, Func<IQueryable<Bank>, IOrderedQueryable<Bank>> orderBy)
		{
			int skip = (page - 1) * pageSize;

			IQueryable<Bank> query = _context.Banks.AsNoTracking();

			if (!string.IsNullOrEmpty(searchString))
			{
				query = query.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
			}

			query = orderBy(query);

			return await query
				.Include(a => a.Company)
					.ThenInclude(ii => ii.Currency)
				.Include(b => b.Currency)
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<int> CreateBankAsync(Bank entity)
		{
			await _context.Banks.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<Bank?> GetBankByIdAsync(int id)
		{
			return await _context.Banks
				.Include(a => a.Company)
					.ThenInclude(ii => ii.Currency)
				.Include(b => b.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<int> UpdateBankAsync(Bank entity)
		{
			_context.Banks.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteBankAsync(Bank entity)
		{
			_context.Banks.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountBanksAsync()
		{
			return await _context.Banks.CountAsync();
		}

		public async Task<bool> IsCurrencyUsedInBanksAsync(int currencyId)
		{
			return await _context.Invoices.AnyAsync(a => a.CurrencyId == currencyId);
		}
	}
}
