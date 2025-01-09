using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models.Directories;
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

		public async Task<IReadOnlyCollection<Bank>> GetAllAsync()
		{
			return await _context.Banks
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Bank?> GetByIdAsync(int id)
		{
			return await _context.Banks
				.Include(a => a.Company)
					.ThenInclude(ii => ii.Currency)
				.Include(b => b.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<bool> IsCurrencyUsedInBanksAsync(int currencyId)
		{
			return await _context.Invoices.AnyAsync(a => a.CurrencyId == currencyId);
		}

	}
}
