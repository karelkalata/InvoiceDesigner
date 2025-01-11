using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Accounting
{
	public class ChartOfAccountsRepository : ABaseRepository<ChartOfAccounts>, IChartOfAccountsRepository
	{
		public ChartOfAccountsRepository(DataContext context) : base(context) { }

		public async Task<ChartOfAccounts?> GetByCodeAsync(int code)
		{
			return await _dbSet
				.FirstOrDefaultAsync(c => c.Code == code);
		}

	}
}
