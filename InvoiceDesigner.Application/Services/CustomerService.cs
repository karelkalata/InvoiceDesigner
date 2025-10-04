using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Application.Services.Abstract;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Services
{
	public class CustomerService : ABaseService<Customer>, ICustomerService
	{
		private readonly ICustomerRepository _repository;

		public CustomerService(ICustomerRepository repository) : base(repository)
		{
			_repository = repository;

		}
		public async Task<ResponsePaged<CustomerViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand)
		{
			var (entities, total) = await GetEntitiesAndCountAsync(pagedCommand);
			var dtos = CustomerMapper.ToViewDto(entities);

			return new ResponsePaged<CustomerViewDto>
			{
				Items = dtos,
				TotalCount = total
			};
		}


		public async Task<ResponseRedirect> CreateAsync(int userId, CustomerEditDto newCustomer)
		{
			var existsCustomer = new Customer();

			MapToCustomer(existsCustomer, newCustomer);

			await _repository.CreateAsync(existsCustomer);

			return new ResponseRedirect
			{
				RedirectUrl = "/Customers"
			};
		}

		public async Task<Customer> GetByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<CustomerEditDto> GetEditDtoByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return CustomerMapper.ToEditDto(existsEntity);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, CustomerEditDto editedCustomer)
		{
			var existsCustomer = await ValidateExistsEntityAsync(editedCustomer.Id);

			MapToCustomer(existsCustomer, editedCustomer);

			await _repository.UpdateAsync(existsCustomer);

			return new ResponseRedirect
			{
				RedirectUrl = "/Customers"
			};
		}

		public Task<int> GetCountAsync() => _repository.GetCountAsync(new GetCountFilter());

		public async Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string searchText)
		{
			var pagedFilter = new PagedFilter
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText,
				SortLabel = "Name",
			};

			var customers = await _repository.GetEntitiesAsync(pagedFilter);
			return CustomerMapper.ToAutocompleteDto(customers);
		}

		private async Task<Customer> ValidateExistsEntityAsync(int id)
		{
			return await _repository.GetByIdAsync(new GetByIdFilter { Id = id })
				?? throw new InvalidOperationException("CustomerId not found");
		}

		private void MapToCustomer(Customer existsCustomer, CustomerEditDto dto)
		{
			existsCustomer.Name = dto.Name.Trim();
			existsCustomer.TaxId = dto.TaxId.Trim();
			existsCustomer.VatId = dto.VatId.Trim();
		}
	}
}
