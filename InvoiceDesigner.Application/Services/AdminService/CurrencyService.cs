using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Application.Services.Abstract;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Services.AdminService
{
	public class CurrencyService : ABaseService<Currency>, ICurrencyService
	{
		private readonly ICurrencyRepository _repository;
		private readonly IInvoiceRepository _invoiceRepository;
		private readonly ICompanyRepository _companyRepository;

		public CurrencyService(ICurrencyRepository repository, IInvoiceRepository invoiceRepository, ICompanyRepository companyRepository) : base(repository)
		{
			_repository = repository;
			_invoiceRepository = invoiceRepository;
			_companyRepository = companyRepository;
		}

		public async Task<ResponsePaged<CurrencyViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand)
		{
			var (entities, total) = await GetEntitiesAndCountAsync(pagedCommand);

			return new ResponsePaged<CurrencyViewDto>
			{
				Items = CurrencyMapper.ToViewDto(entities),
				TotalCount = total
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, CurrencyEditDto newCurrency)
		{
			var existingCurrency = new Currency();
			MapToCurrency(existingCurrency, newCurrency);

			await _repository.CreateAsync(existingCurrency);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public async Task<CurrencyEditDto> GetEditDtoByIdAsync(int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(id);
			return CurrencyMapper.ToEditDto(existsEntity);
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
			MapToCurrency(existingCurrency, editedCurrency);

			await _repository.UpdateAsync(existingCurrency);

			return new ResponseRedirect
			{
				RedirectUrl = "/Currencies"
			};
		}

		public override async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand)
		{
			if (await _invoiceRepository.IsCurrencyUsed(deleteEntityCommand.EntityId))
				throw new InvalidOperationException("Currency is in use in Invoices and cannot be deleted.");

			if (await _companyRepository.IsCurrencyUsedI(deleteEntityCommand.EntityId))
				throw new InvalidOperationException($"Currencyis in use in Company and cannot be deleted.");

			return await base.DeleteOrMarkAsDeletedAsync(deleteEntityCommand);
		}

		public Task<int> GetCountAsync()
		{
			return _repository.GetCountAsync(new GetCountFilter());
		}

		public async Task<IReadOnlyCollection<CurrencyAutocompleteDto>> GetAutocompleteDto()
		{
			var currencies = await _repository.GetAllAsync();
			return CurrencyMapper.ToAutocompleteDto(currencies);

		}

		public async Task<IReadOnlyCollection<CurrencyAutocompleteDto>> FilteringData(string searchText)
		{
			var pagedFilter = new PagedFilter
			{
				PageSize = 10,
				Page = 1,
				ExcludeString = searchText,
				SortLabel = "Name",
			};

			var currencies = await _repository.GetEntitiesAsync(pagedFilter);
			return CurrencyMapper.ToAutocompleteDto(currencies);
		}

		private async Task<Currency> ValidateExistsEntityAsync(int id)
		{
			var existsEntity = await _repository.GetByIdAsync(new GetByIdFilter { Id = id });
			if (existsEntity == null)
				throw new InvalidOperationException("Currency not found");

			return existsEntity;
		}

		private void MapToCurrency(Currency existingCurrency, CurrencyEditDto dto)
		{
			existingCurrency.Name = dto.Name.ToUpper();
			existingCurrency.Description = dto.Description.Trim();
		}

	}
}
