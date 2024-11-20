using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _repository;
		private readonly IMapper _mapper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;

		public CustomerService(ICustomerRepository repository,
								IMapper mapper,
								IInvoiceServiceHelper invoiceServiceHelper)
		{
			_repository = repository;
			_mapper = mapper;
			_invoiceServiceHelper = invoiceServiceHelper;
		}

		public async Task<ResponsePaged<CustomerViewDto>> GetPagedCustomersAsync(int pageSize, int page, string searchString, string sortLabel, bool showDeleted = false)
		{
			pageSize = Math.Max(pageSize, 1);
			page = Math.Max(page, 1);

			var clientsTask = _repository.GetCustomersAsync(pageSize, page, searchString, GetOrdering(sortLabel), showDeleted);
			var totalCountTask = _repository.GetCountCustomersAsync(showDeleted);

			await Task.WhenAll(clientsTask, totalCountTask);

			var clientsViewDto = _mapper.Map<IReadOnlyCollection<CustomerViewDto>>(await clientsTask);

			return new ResponsePaged<CustomerViewDto>
			{
				Items = clientsViewDto,
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateCustomerAsync(CustomerEditDto newCustomer)
		{
			var existsCustomer = new Customer();

			MapCustomer(existsCustomer, newCustomer);

			var entityId = await _repository.CreateCustomerAsync(existsCustomer);
			return new ResponseRedirect
			{
				RedirectUrl = "/Customers"
			};
		}

		public async Task<Customer> GetCustomerByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<CustomerEditDto> GetCustomerEditDtoByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<CustomerEditDto>(existsEntity);
		}

		public async Task<ResponseRedirect> UpdateCustomerAsync(CustomerEditDto editedCustomer)
		{
			var existsCustomer = await ValidateExistsEntityAsync(editedCustomer.Id);

			MapCustomer(existsCustomer, editedCustomer);

			var entityId = await _repository.UpdateCustomerAsync(existsCustomer);
			return new ResponseRedirect
			{
				RedirectUrl = "/Customers"
			};
		}

		public async Task<ResponseBoolean> DeleteCustomerAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);

			if (await _invoiceServiceHelper.IsClientUsedInInvoices(id))
				throw new InvalidOperationException($"Customer {existsEntity.Name} is in use in Invoices and cannot be deleted.");

			return new ResponseBoolean
			{
				Result = await _repository.DeleteCustomerAsync(existsEntity)
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int id, int modeDelete)
		{
			if (modeDelete == 0)
			{
				var existsEntity = await ValidateExistsEntityAsync(id);
				existsEntity.IsDeleted = true;

				await _repository.UpdateCustomerAsync(existsEntity);

				return new ResponseBoolean { Result = true };
			}
			return await DeleteCustomerAsync(id);
		}

		public Task<int> GetCustomerCountAsync() => _repository.GetCountCustomersAsync();

		public async Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string f)
		{
			var customersTask = _repository.GetCustomersAsync(10, 1, f, GetOrdering("Value"));
			var customers = await customersTask;
			return _mapper.Map<IReadOnlyCollection<CustomerAutocompleteDto>>(customers);
		}

		private async Task<Customer> ValidateExistsEntityAsync(int id)
		{
			return await _repository.GetCustomerByIdAsync(id)
				?? throw new InvalidOperationException("Customer not found");
		}

		private void MapCustomer(Customer existsCustomer, CustomerEditDto dto)
		{
			existsCustomer.Name = dto.Name.Trim();
			existsCustomer.TaxId = dto.TaxId.Trim();
			existsCustomer.VatId = dto.VatId.Trim();
		}

		private Func<IQueryable<Customer>, IOrderedQueryable<Customer>> GetOrdering(string sortLabel)
		{
			var sortingOptions = new Dictionary<string, Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>
			{
				{ "Id_desc", q => q.OrderByDescending(e => e.Id) },
				{ "Name", q => q.OrderBy(e => e.Name) },
				{ "Name_desc", q => q.OrderByDescending(e => e.Name) }
			};

			return sortingOptions.TryGetValue(sortLabel, out var orderFunc) ? orderFunc : q => q.OrderBy(e => e.Id);
		}
	}

}
