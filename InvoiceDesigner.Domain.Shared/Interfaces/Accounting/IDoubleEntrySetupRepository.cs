using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Accounting
{
	public interface IDoubleEntrySetupRepository
	{
		Task<IReadOnlyCollection<DoubleEntrySetup>> GetEntitiesAsync(QueryPagedDoubleEntrySetup queryPaged);
		Task<int> CreateAsync(DoubleEntrySetup entity);
		Task<DoubleEntrySetup?> GetByIdAsync(int id);
		Task<int> UpdateAsync(DoubleEntrySetup entity);
		Task<bool> DeleteAsync(DoubleEntrySetup entity);
		Task<int> GetCountAsync();
		Task<int> GetCountByTypeDocumentAsync(EAccountingDocument typeDocument);
	}
}
