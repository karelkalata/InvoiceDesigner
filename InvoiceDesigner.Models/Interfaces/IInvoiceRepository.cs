using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IInvoiceRepository
	{
		Task<IReadOnlyCollection<Invoice>> GetInvoicesAsync(QueryPaged queryPaged,
											Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy,
											IReadOnlyCollection<Company> userAuthorizedCompanies);

		Task<int> CreateInvoiceAsync(Invoice entity);

		Task<Invoice?> GetInvoiceByIdAsync(int id, IReadOnlyCollection<Company> userAuthorizedCompanies);

		Task<int> UpdateInvoiceAsync(Invoice entity);

		Task<bool> DeleteInvoiceAsync(Invoice entity);

		Task<int> GetCountInvoicesAsync(QueryPaged queryPaged, IReadOnlyCollection<Company> userAuthorizedCompanies);

		Task<int> GetNextInvoiceNumberForCompanyAsync(int companyId);

		Task<bool> IsCompanyUsedInInvoices(int companyId);

		Task<bool> IsBankUsedInInvoices(int bankId);

		Task<bool> IsClientUsedInInvoices(int clientId);

		Task<bool> IsCurrencyUsedInInvoices(int currencyId);

		Task<bool> IsProductUsedInInvoiceItems(int productId);

	}
}
