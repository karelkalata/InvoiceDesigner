using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Directories
{
	public class CompanyRepository : ABaseRepository<Company>, ICompanyRepository
	{
		public CompanyRepository(DataContext context) : base(context) { }

		public override async Task<Company?> GetByIdAsync(int id)
		{
			return await _dbSet
				.Include(c => c.Currency)
				.Include(c => c.Banks)
					.ThenInclude(c => c.Currency)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<IReadOnlyCollection<Company>> GetAllCompaniesDto()
		{
			return await _dbSet
				.Include(a => a.Currency)
				.Include(b => b.Banks)
					.ThenInclude(c => c.Currency)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<bool> IsCurrencyUsedInCompany(int currencyId)
		{
			return await _dbSet
				.AnyAsync(c => c.CurrencyId == currencyId);
		}
	}
}
