using InvoiceDesigner.Domain.Shared.DTOs.Reports.TrialBalance;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.Reports
{
	public interface ITrialBalanceService
	{
		Task<ResponsePaged<ReportTrialBalance>> GetPagedAsync(QueryTrialBalance queryPaged);
	}
}
