using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services
{
	public class CurrencyService : ICurrencyService
	{
		private readonly ICurrencyRepository _repository;
		private readonly IMapper _mapper;
		private readonly IBankServiceHelper _bankServiceHelper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly ICompanyServiceHelper _companyServiceHelper;
		private readonly IProductServiceHelper _productServiceHelper;

		public CurrencyService(ICurrencyRepository repository,
								IMapper mapper,
								IBankServiceHelper bankServiceHelper,
								IInvoiceServiceHelper invoiceServiceHelper,
								ICompanyServiceHelper companyServiceHelper,
								IProductServiceHelper productServiceHelper)
		{
			_repository = repository;
			_mapper = mapper;
			_bankServiceHelper = bankServiceHelper;
			_invoiceServiceHelper = invoiceServiceHelper;
			_companyServiceHelper = companyServiceHelper;
			_productServiceHelper = productServiceHelper;
		}

		public async Task<ResponsePaged<CurrencyViewDto>> GetPagedCurrenciesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var currenciesTask = _repository.GetCurrenciesAsync(queryPaged, GetOrdering(queryPaged.SortLabel));
			var totalCountTask = _repository.GetCountCurrenciesAsync(queryPaged.ShowDeleted);

			await Task.WhenAll(currenciesTask, totalCountTask);

			var currenciesViewDto = _mapper.Map<IReadOnlyCollection<CurrencyViewDto>>(await currenciesTask);

			return new ResponsePaged<CurrencyViewDto>
			{
				Items = currenciesViewDto,
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateCurrencyAsync(CurrencyEditDto newCurrency)
		{
			var existingCurrency = new Currency();
			MapCurrency(existingCurrency, newCurrency);

			var entityId = await _repository.CreateCurrencyAsync(existingCurrency);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public async Task<CurrencyEditDto> GetCurrencyEditDtoByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return _mapper.Map<CurrencyEditDto>(existsEntity);
		}

		public async Task<Currency> GetCurrencyByIdAsync(int id) => await ValidateExistsEntityAsync(id);


		public async Task<ResponseRedirect> UpdateCurrencyAsync(CurrencyEditDto editedCurrency)
		{
			var existingCurrency = await ValidateExistsEntityAsync(editedCurrency.Id);
			MapCurrency(existingCurrency, editedCurrency);

			var entityId = await _repository.UpdateCurrencyAsync(existingCurrency);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public async Task<ResponseBoolean> DeleteCurrencyAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);

			if (await _bankServiceHelper.IsCurrencyUsedInBanks(id))
				throw new InvalidOperationException($"{existsEntity.Name} is in use in Banks and cannot be deleted.");

			if (await _invoiceServiceHelper.IsCurrencyUsedInInvoices(id))
				throw new InvalidOperationException($"{existsEntity.Name} is in use in Invoices and cannot be deleted.");

			if (await _companyServiceHelper.IsCurrencyUsedInCompany(id))
				throw new InvalidOperationException($"{existsEntity.Name} is in use in Company and cannot be deleted.");

			if (await _productServiceHelper.IsCurrencyUsedInProduct(id))
				throw new InvalidOperationException($"{existsEntity.Name} is in use in Products and cannot be deleted.");

			return new ResponseBoolean
			{
				Result = await _repository.DeleteCurrencyAsync(existsEntity)
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int id, int modeDelete)
		{
			if (modeDelete == 0)
			{
				var existsEntity = await ValidateExistsEntityAsync(id);
				existsEntity.IsDeleted = true;

				await _repository.UpdateCurrencyAsync(existsEntity);

				return new ResponseBoolean { Result = true };
			}
			return await DeleteCurrencyAsync(id);
		}

		public Task<int> GetCountCurrenciesAsync()
		{
			return _repository.GetCountCurrenciesAsync();
		}

		public async Task<IReadOnlyCollection<CurrencyAutocompleteDto>> GetCurrencyAutocompleteDto()
		{
			var currencies = await _repository.GetAllCurrenciesAsync();
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

			var currencies = await _repository.GetCurrenciesAsync(queryPaged, GetOrdering("Value"));
			return _mapper.Map<IReadOnlyCollection<CurrencyAutocompleteDto>>(currencies);
		}

		private async Task<Currency> ValidateExistsEntityAsync(int id)
		{
			var existsEntity = await _repository.GetCurrencyByIdAsync(id);
			if (existsEntity == null)
				throw new InvalidOperationException("ValidateExistsEntityAsync: Not Found");

			return existsEntity;
		}

		private void MapCurrency(Currency existingCurrency, CurrencyEditDto dto)
		{
			existingCurrency.Name = dto.Name.ToUpper();
			existingCurrency.Description = dto.Description.Trim();
		}

		private Func<IQueryable<Currency>, IOrderedQueryable<Currency>> GetOrdering(string sortLabel)
		{
			var sortingOptions = new Dictionary<string, Func<IQueryable<Currency>, IOrderedQueryable<Currency>>>
		{
			{"Id_desc", q => q.OrderByDescending(e => e.Id)},
			{"Code", q => q.OrderBy(e => e.Name)},
			{"Code_desc", q => q.OrderByDescending(e => e.Name)},
			{"Id", q => q.OrderBy(e => e.Id)}
		};

			return sortingOptions.TryGetValue(sortLabel, out var orderFunc) ? orderFunc : sortingOptions["Id"];
		}
	}

}
