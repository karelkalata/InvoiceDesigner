using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICompanyRepository
	{
		Task<IReadOnlyCollection<Company>> GetCompaniesAsync(QueryPaged queryPaged,
																Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy);

		Task<int> CreateCompanyAsync(Company entity);

		Task<Company?> GetCompanyByIdAsync(int id);

		Task<int> UpdateCompanyAsync(Company entity);

		Task<bool> DeleteCompanyAsync(Company entity);

		Task<int> GetCountCompaniesAsync();

		Task<IReadOnlyCollection<Company>> GetAllCompaniesDto();

		Task<bool> IsCurrencyUsedInCompany(int currencyId);
	}
}
