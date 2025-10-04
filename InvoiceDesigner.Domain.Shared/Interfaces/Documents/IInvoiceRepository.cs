using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Documents
{
	public interface IInvoiceRepository : IABaseRepository<Invoice>
	{
		Task<bool> IsProductUsed(int productId);



		Task<int> GetCountAsync(PagedFilter pagedFilter);
		Task<int> GetNextNumberForCompanyAsync(int companyId);
		Task<bool> IsCompanyUsed(int companyId);
		Task<bool> IsBankUsed(int bankId);
		Task<bool> IsClientUsed(int clientId);
		Task<bool> IsCurrencyUsed(int currencyId);

	}
}
