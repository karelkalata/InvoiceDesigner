using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICompanyRepository
	{
		Task<IReadOnlyCollection<Company>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<Company>, IOrderedQueryable<Company>> orderBy);

		Task<int> CreateAsync(Company entity);

		Task<Company?> GetByIdAsync(int id);

		Task<int> UpdateAsync(Company entity);

		Task<bool> DeleteAsync(Company entity);

		Task<int> GetCountAsync();

		Task<IReadOnlyCollection<Company>> GetAllCompaniesDto();

		Task<bool> IsCurrencyUsedInCompany(int currencyId);

	}
}
