using InvoiceDesigner.Domain.Shared.DTOs.Reports.TrialBalance;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Reports
{
	public interface ITrialBalanceRepository
	{
		Task<IReadOnlyCollection<TrialBalanceDto>> GetAsync(QueryTrialBalance queryPaged);
	}
}
