using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICustomerRepository
	{

		Task<IReadOnlyCollection<Customer>> GetCustomersAsync(QueryPaged queryPaged,
																Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy);

		Task<int> CreateCustomerAsync(Customer entity);

		Task<Customer?> GetCustomerByIdAsync(int id);

		Task<int> UpdateCustomerAsync(Customer entity);

		Task<bool> DeleteCustomerAsync(Customer entity);

		Task<int> GetCountCustomersAsync(bool showDeleted = false);
	}
}
