using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _repoCustomer;
		private readonly IMapper _mapper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly IUserActivityLogService _userActivity;

		public CustomerService(ICustomerRepository repoCustomer, 
								IMapper mapper, 
								IInvoiceServiceHelper invoiceServiceHelper,
								IUserActivityLogService userActivity)
		{
			_repoCustomer = repoCustomer;
			_mapper = mapper;
			_invoiceServiceHelper = invoiceServiceHelper;
			_userActivity = userActivity;
		}

		public async Task<ResponsePaged<CustomerViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var clientsTask = _repoCustomer.GetEntitiesAsync(queryPaged, GetOrdering(queryPaged.SortLabel));
			var totalCountTask = _repoCustomer.GetCountAsync(queryPaged.ShowDeleted);

			await Task.WhenAll(clientsTask, totalCountTask);

			var clientsViewDto = _mapper.Map<IReadOnlyCollection<CustomerViewDto>>(await clientsTask);

			return new ResponsePaged<CustomerViewDto>
			{
				Items = clientsViewDto,
				TotalCount = await totalCountTask
			};
		}


		public async Task<ResponseRedirect> CreateAsync(int userId, CustomerEditDto newCustomer)
		{
			var existsCustomer = new Customer();

			MapCustomer(existsCustomer, newCustomer);

			var entityId = await _repoCustomer.CreateAsync(existsCustomer);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.Customer, EActivitiesType.Create, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = "/Customers"
			};
		}

		public async Task<Customer> GetByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<CustomerEditDto> GetEditDtoByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<CustomerEditDto>(existsEntity);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, CustomerEditDto editedCustomer)
		{
			var existsCustomer = await ValidateExistsEntityAsync(editedCustomer.Id);

			MapCustomer(existsCustomer, editedCustomer);

			var entityId = await _repoCustomer.UpdateAsync(existsCustomer);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.Customer, EActivitiesType.Update, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = "/Customers"
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryDeleteEntity.EntityId);

			if (!queryDeleteEntity.MarkAsDeleted)
			{
				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.Customer, EActivitiesType.Delete, existsEntity.Id);

				return new ResponseBoolean
				{
					Result = await _repoCustomer.DeleteAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoCustomer.UpdateAsync(existsEntity);
				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.Customer, EActivitiesType.MarkedAsDeleted, existsEntity.Id);

				return new ResponseBoolean
				{
					Result = true
				};
			}
		}

		public Task<int> GetCountAsync() => _repoCustomer.GetCountAsync();

		public async Task<IReadOnlyCollection<CustomerAutocompleteDto>> FilteringData(string searchText)
		{
			var queryPaged = new QueryPaged
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText
			};

			var customers = await _repoCustomer.GetEntitiesAsync(queryPaged, GetOrdering("Value"));
			return _mapper.Map<IReadOnlyCollection<CustomerAutocompleteDto>>(customers);
		}

		private async Task<Customer> ValidateExistsEntityAsync(int id)
		{
			return await _repoCustomer.GetByIdAsync(id)
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
