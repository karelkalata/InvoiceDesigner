using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Services
{
	public class BankService : IBankService
	{
		private readonly IBankRepository _repository;
		private readonly IMapper _mapper;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly ICompanyService _companyService;
		private readonly ICurrencyService _currencyService;

		public BankService(IBankRepository repository, IMapper mapper, IInvoiceServiceHelper invoiceServiceHelper,
						   ICompanyService companyService, ICurrencyService currencyService)
		{
			_repository = repository;
			_mapper = mapper;
			_invoiceServiceHelper = invoiceServiceHelper;
			_companyService = companyService;
			_currencyService = currencyService;
		}

		public async Task<PagedResult<BankViewDto>> GetPagedBanksAsync(int pageSize, int page, string searchString, string sortLabel)
		{
			pageSize = Math.Max(pageSize, 1);
			page = Math.Max(page, 1);

			var banksTask = _repository.GetBanksAsync(pageSize, page, searchString, GetOrdering(sortLabel));
			var totalCountTask = _repository.GetCountBanksAsync();

			await Task.WhenAll(banksTask, totalCountTask);

			var bankDtos = _mapper.Map<IReadOnlyCollection<BankViewDto>>(await banksTask);
			return new PagedResult<BankViewDto> { Items = bankDtos, TotalCount = await totalCountTask };
		}

		public async Task<ResponseRedirect> CreateBankAsync(BankEditDto newBank)
		{
			var (currency, company) = await ValidateInputAsync(newBank);

			var existingBank = new Bank();
			MapBank(existingBank, newBank, currency, company);

			var entityId = await _repository.CreateBankAsync(existingBank);

			return new ResponseRedirect 
			{ 
				RedirectUrl = "/Banks"
			};
		}

		public async Task<Bank?> GetBankByIdAsync(int id)
		{
			return await _repository.GetBankByIdAsync(id);
		}

		public async Task<BankEditDto> GetBankEditDtoByIdAsync(int id)
		{
			var bank = await ValidateExistsEntityAsync(id);
			return _mapper.Map<BankEditDto>(bank);
		}

		public async Task<ResponseRedirect> UpdateBankAsync(BankEditDto editedBank)
		{
			var existingBank = await ValidateExistsEntityAsync(editedBank.Id);
			var (currency, company) = await ValidateInputAsync(editedBank);

			MapBank(existingBank, editedBank, currency, company);

			var entityId = await _repository.UpdateBankAsync(existingBank);

			return new ResponseRedirect
			{
				RedirectUrl = "/Banks"
			};
		}

		public async Task<bool> DeleteBankAsync(int id)
		{
			var bank = await ValidateExistsEntityAsync(id);
			if (await _invoiceServiceHelper.IsBankUsedInInvoices(id))
				throw new InvalidOperationException($"Bank {bank.Name} is used in invoices and cannot be deleted.");

			return await _repository.DeleteBankAsync(bank);
		}

		public async Task<int> GetBanksCountAsync() => await _repository.GetCountBanksAsync();

		public async Task<IReadOnlyCollection<BankAutocompleteDto>> GetAllBanksAutocompleteDto()
		{
			var banks = await _repository.GetAllBanksAsync();
			return _mapper.Map<IReadOnlyCollection<BankAutocompleteDto>>(banks);
		}

		private async Task<Bank> ValidateExistsEntityAsync(int id)
		{
			var bank = await _repository.GetBankByIdAsync(id)
						?? throw new InvalidOperationException("Bank not found");
			return bank;
		}

		private async Task<(Currency, Company)> ValidateInputAsync(BankEditDto bankEditDto)
		{
			var currency = await _currencyService.GetCurrencyByIdAsync(bankEditDto.Currency.Id)
						   ?? throw new InvalidOperationException($"Currency ID {bankEditDto.Currency.Id} not found.");
			var company = await _companyService.GetCompanyByIdAsync(bankEditDto.Company.Id)
						   ?? throw new InvalidOperationException($"Company ID {bankEditDto.Company.Id} not found.");
			return (currency, company);
		}

		private void MapBank(Bank existingBank, BankEditDto dto, Currency currency, Company company)
		{
			existingBank.Name = dto.Name.Trim();
			existingBank.Address = dto.Address.Trim();
			existingBank.BIC = dto.BIC.Trim();
			existingBank.Account = dto.Account.Trim();
			existingBank.CurrencyId = currency.Id;
			existingBank.CompanyId = company.Id;
			existingBank.Currency = currency;
			existingBank.Company = company;
		}

		private Func<IQueryable<Bank>, IOrderedQueryable<Bank>> GetOrdering(string sortLabel)
		{
			var orderingOptions = new Dictionary<string, Func<IQueryable<Bank>, IOrderedQueryable<Bank>>>
			{
				{"Id_desc", q => q.OrderByDescending(e => e.Id)},
				{"Name", q => q.OrderBy(e => e.Name)},
				{"Name_desc", q => q.OrderByDescending(e => e.Name)}
			};

			return orderingOptions.GetValueOrDefault(sortLabel, q => q.OrderBy(e => e.Id));
		}
	}

}
