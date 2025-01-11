using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Accounting
{
	public interface IChartOfAccountsRepository : IABaseRepository<ChartOfAccounts>
	{
		Task<ChartOfAccounts?> GetByCodeAsync(int code);
	}
}
