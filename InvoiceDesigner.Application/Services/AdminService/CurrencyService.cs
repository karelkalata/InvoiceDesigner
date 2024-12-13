using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.AdminService
{
	public class CurrencyService : ICurrencyService
	{
		private readonly ICurrencyRepository _repoCurrency;
		private readonly IMapper _mapper;
		private readonly IBankServiceHelper _bankServiceHelper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly ICompanyServiceHelper _companyServiceHelper;
		private readonly IProductServiceHelper _productServiceHelper;
		private readonly IUserActivityLogService _userActivity;

		public CurrencyService(ICurrencyRepository repoCurrency,
								IMapper mapper,
								IBankServiceHelper bankServiceHelper,
								IInvoiceServiceHelper invoiceServiceHelper,
								ICompanyServiceHelper companyServiceHelper,
								IProductServiceHelper productServiceHelper,
								IUserActivityLogService userActivity)
		{
			_repoCurrency = repoCurrency;
			_mapper = mapper;
			_bankServiceHelper = bankServiceHelper;
			_invoiceServiceHelper = invoiceServiceHelper;
			_companyServiceHelper = companyServiceHelper;
			_productServiceHelper = productServiceHelper;
			_userActivity = userActivity;
		}

		public async Task<ResponsePaged<CurrencyViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var currenciesTask = _repoCurrency.GetEntitiesAsync(queryPaged, GetOrdering(queryPaged.SortLabel));
			var totalCountTask = _repoCurrency.GetCountAsync(queryPaged.ShowDeleted);

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
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.Currency, EActivitiesType.Create, entityId);

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


		public async Task<ResponseRedirect> UpdateAsync(int userId, CurrencyEditDto editedCurrency)
		{
			var existingCurrency = await ValidateExistsEntityAsync(editedCurrency.Id);
			MapCurrency(existingCurrency, editedCurrency);

			var entityId = await _repoCurrency.UpdateAsync(existingCurrency);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.Currency, EActivitiesType.Update, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryDeleteEntity.EntityId);

			if (!queryDeleteEntity.MarkAsDeleted)
			{
				if (await _bankServiceHelper.IsCurrencyUsedInBanks(queryDeleteEntity.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Banks and cannot be deleted.");

				if (await _invoiceServiceHelper.IsCurrencyUsedInInvoices(queryDeleteEntity.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Invoices and cannot be deleted.");

				if (await _companyServiceHelper.IsCurrencyUsedInCompany(queryDeleteEntity.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Company and cannot be deleted.");

				if (await _productServiceHelper.IsCurrencyUsedInProduct(queryDeleteEntity.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Products and cannot be deleted.");

				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.Currency, EActivitiesType.Delete, existsEntity.Id);

				return new ResponseBoolean
				{
					Result = await _repoCurrency.DeleteAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoCurrency.UpdateAsync(existsEntity);
				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.Currency, EActivitiesType.MarkedAsDeleted, existsEntity.Id);

				return new ResponseBoolean
				{
					Result = true
				};
			}
		}

		public Task<int> GetCountAsync()
		{
			return _repoCurrency.GetCountAsync();
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

			var currencies = await _repoCurrency.GetEntitiesAsync(queryPaged, GetOrdering("Value"));

			return _mapper.Map<IReadOnlyCollection<CurrencyAutocompleteDto>>(currencies);
		}

		private async Task<Currency> ValidateExistsEntityAsync(int id)
		{
			var existsEntity = await _repoCurrency.GetByIdAsync(id);
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
