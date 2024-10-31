using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface ICustomerRepository
	{

		Task<IReadOnlyCollection<Customer>> GetCustomersAsync(int pageSize,
															int pageNumber,
															string searchString,
															Func<IQueryable<Customer>, IOrderedQueryable<Customer>> orderBy);

		Task<int> CreateCustomerAsync(Customer entity);

		Task<Customer?> GetCustomerByIdAsync(int id);

		Task<int> UpdateCustomerAsync(Customer entity);

		Task<bool> DeleteCustomerAsync(Customer entity);

		Task<int> GetCountCustomersAsync();

	}
}
