using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class BankRepository : ABaseRepository<Bank>, IBankRepository
	{
		public BankRepository(DataContext context) : base(context) { }

		public override async Task<Bank?> GetByIdAsync(int id)
		{
			return await _dbSet
				.Include(a => a.Company)
					.ThenInclude(ii => ii.Currency)
				.Include(b => b.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<IReadOnlyCollection<Bank>> GetAllAsync()
		{
			return await _dbSet
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<bool> IsCurrencyUsedInBanksAsync(int currencyId)
		{
			return await _dbSet
				.AnyAsync(a => a.CurrencyId == currencyId);
		}

	}
}
