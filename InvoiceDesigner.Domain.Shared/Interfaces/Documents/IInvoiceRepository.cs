using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Documents
{
	public interface IInvoiceRepository
	{
		Task<IReadOnlyCollection<Invoice>> GetAsync(QueryPaged queryPaged,
											Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy,
											IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<int> CreateAsync(Invoice entity);
		Task<Invoice?> GetByIdAsync(int id, IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<int> UpdateAsync(Invoice entity);
		Task<bool> DeleteAsync(Invoice entity);
		Task<int> GetCountAsync(QueryPaged queryPaged, IReadOnlyCollection<Company> userAuthorizedCompanies);
		Task<int> GetNextNumberForCompanyAsync(int companyId);
		Task<bool> IsCompanyUsed(int companyId);
		Task<bool> IsBankUsed(int bankId);
		Task<bool> IsClientUsed(int clientId);
		Task<bool> IsCurrencyUsed(int currencyId);
		Task<bool> IsProductUsedInInvoiceItems(int productId);

	}
}
