using AutoMapper;
using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Records;

namespace InvoiceDesigner.Application.Services.AdminService
{
	public class CurrencyService : ICurrencyService
	{
		private readonly ICurrencyRepository _repoCurrency;
		private readonly IMapper _mapper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly ICompanyServiceHelper _companyServiceHelper;
		private readonly IProductServiceHelper _productServiceHelper;

		public CurrencyService(ICurrencyRepository repoCurrency,
								IMapper mapper,
								IInvoiceServiceHelper invoiceServiceHelper,
								ICompanyServiceHelper companyServiceHelper,
								IProductServiceHelper productServiceHelper)
		{
			_repoCurrency = repoCurrency;
			_mapper = mapper;
			_invoiceServiceHelper = invoiceServiceHelper;
			_companyServiceHelper = companyServiceHelper;
			_productServiceHelper = productServiceHelper;
		}

		public async Task<ResponsePaged<CurrencyViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var currenciesTask = _repoCurrency.GetEntitiesAsync(queryPaged, queryPaged.SortLabel);

			var recordGetCount = new GetCountFilter
			{
				ShowDeleted = queryPaged.ShowDeleted,
				ShowArchived = queryPaged.ShowArchived
			};
			var totalCountTask = _repoCurrency.GetCountAsync(recordGetCount);

			await Task.WhenAll(currenciesTask, totalCountTask);

			var currenciesViewDto = _mapper.Map<IReadOnlyCollection<CurrencyViewDto>>(await currenciesTask);

			return new ResponsePaged<CurrencyViewDto>
			{
				Items = currenciesViewDto,
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, CurrencyEditDto newCurrency)
		{
			var existingCurrency = new Currency();
			MapCurrency(existingCurrency, newCurrency);

			var entityId = await _repoCurrency.CreateAsync(existingCurrency);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public async Task<CurrencyEditDto> GetEditDtoByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<CurrencyEditDto>(existsEntity);
		}

		public async Task<Currency> GetByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<string> GetNameByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return existsEntity.Name;
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, CurrencyEditDto editedCurrency)
		{
			var existingCurrency = await ValidateExistsEntityAsync(editedCurrency.Id);
			MapCurrency(existingCurrency, editedCurrency);

			await _repoCurrency.UpdateAsync(existingCurrency);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand)
		{
			var existsEntity = await ValidateExistsEntityAsync(deleteEntityCommand.EntityId);

			if (!deleteEntityCommand.MarkAsDeleted)
			{

				if (await _invoiceServiceHelper.IsCurrencyUsedInInvoices(deleteEntityCommand.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Invoices and cannot be deleted.");

				if (await _companyServiceHelper.IsCurrencyUsedInCompany(deleteEntityCommand.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Company and cannot be deleted.");

				return new ResponseBoolean
				{
					Result = await _repoCurrency.DeleteAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoCurrency.UpdateAsync(existsEntity);

				return new ResponseBoolean
				{
					Result = true
				};
			}
		}

		public Task<int> GetCountAsync()
		{
			return _repoCurrency.GetCountAsync(new GetCountFilter());
		}

		public async Task<IReadOnlyCollection<CurrencyAutocompleteDto>> GetAutocompleteDto()
		{
			var currencies = await _repoCurrency.GetAllAsync();
			return _mapper.Map<IReadOnlyCollection<CurrencyAutocompleteDto>>(currencies);
		}

		public async Task<IReadOnlyCollection<CurrencyAutocompleteDto>> FilteringData(string searchText)
		{
			var queryPaged = new QueryPaged
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText
			};

			var currencies = await _repoCurrency.GetEntitiesAsync(queryPaged, "Name");

			return _mapper.Map<IReadOnlyCollection<CurrencyAutocompleteDto>>(currencies);
		}

		private async Task<Currency> ValidateExistsEntityAsync(int id)
		{
			var existsEntity = await _repoCurrency.GetByIdAsync(id);
			if (existsEntity == null)
				throw new InvalidOperationException("Currency not found");

			return existsEntity;
		}

		private void MapCurrency(Currency existingCurrency, CurrencyEditDto dto)
		{
			existingCurrency.Name = dto.Name.ToUpper();
			existingCurrency.Description = dto.Description.Trim();
		}

	}
}
