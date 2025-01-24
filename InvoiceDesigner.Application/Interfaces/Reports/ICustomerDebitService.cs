using InvoiceDesigner.Domain.Shared.DTOs.Reports.CustomerDebit;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.Reports
{
	public interface ICustomerDebitService
	{
		Task<ResponsePaged<ReportCustomerDebit>> GetPagedAsync(QueryCustomerDebit queryPaged);
	}
}
