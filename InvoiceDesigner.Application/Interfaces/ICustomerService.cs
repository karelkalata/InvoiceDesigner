using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICustomerService
	{
		Task<ResponsePaged<CustomerViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateAsync(int userId, CustomerEditDto newEntity);

		Task<Customer> GetByIdAsync(int id);

		Task<CustomerEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, CustomerEditDto editedEntity);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity);

		Task<int> GetCountAsync();

		Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string f);

	}
}
