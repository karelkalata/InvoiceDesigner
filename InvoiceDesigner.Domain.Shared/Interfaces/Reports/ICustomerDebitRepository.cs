using InvoiceDesigner.Domain.Shared.DTOs.Reports.CustomerDebit;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Reports
{
	public interface ICustomerDebitRepository
	{
		Task<IReadOnlyCollection<CustomerDebitDto>> GetAsync(QueryCustomerDebit queryPaged, ChartOfAccounts chartOfAccounts);
	}
}
