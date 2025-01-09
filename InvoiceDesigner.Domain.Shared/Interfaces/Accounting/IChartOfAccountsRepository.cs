using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Accounting
{
	public interface IChartOfAccountsRepository
	{
		Task<IReadOnlyCollection<ChartOfAccounts>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<ChartOfAccounts>, IOrderedQueryable<ChartOfAccounts>> orderBy);
		Task<int> CreateAsync(ChartOfAccounts entity);
		Task<ChartOfAccounts?> GetByIdAsync(int id);
		Task<ChartOfAccounts?> GetByCodeAsync(int code);
		Task<int> UpdateAsync(ChartOfAccounts entity);
		Task<bool> DeleteAsync(ChartOfAccounts entity);
		Task<int> GetCountAsync();
	}
}
