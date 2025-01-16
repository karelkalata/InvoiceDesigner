using InvoiceDesigner.Domain.Shared.DTOs.Reports;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Reports
{
	public interface ITrialBalanceRepository
	{
		Task<IReadOnlyCollection<TrialBalanceDto>> GetAsync(QueryTrialBalance queryPaged);
	}
}
