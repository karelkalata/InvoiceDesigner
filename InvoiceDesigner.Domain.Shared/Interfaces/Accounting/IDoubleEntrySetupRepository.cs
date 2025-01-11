using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Accounting
{
	public interface IDoubleEntrySetupRepository : IABaseRepository<DoubleEntrySetup>
	{
		Task<IReadOnlyCollection<DoubleEntrySetup>> GetEntitiesAsync(QueryPagedDoubleEntrySetup queryPaged);
		Task<int> GetCountByTypeDocumentAsync(EAccountingDocument AccountingDocument);
	}
}
