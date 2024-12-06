using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICustomerRepository
	{

		Task<IReadOnlyCollection<Customer>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy);

		Task<int> CreateAsync(Customer entity);

		Task<Customer?> GetByIdAsync(int id);

		Task<int> UpdateAsync(Customer entity);

		Task<bool> DeleteAsync(Customer entity);

		Task<int> GetCountAsync(bool showDeleted = false);
	}
}
