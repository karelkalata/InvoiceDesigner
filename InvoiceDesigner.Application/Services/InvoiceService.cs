using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IInvoiceRepository _repository;
		private readonly IMapper _mapper;
		private readonly ICompanyService _companyService;
		private readonly ICurrencyService _currencyService;
		private readonly IProductService _productService;
		private readonly IBankService _bankService;
		private readonly ICustomerService _customerService;
		private readonly IUserAuthorizedDataService _userAuthorizedDataService;

		public InvoiceService(IInvoiceRepository repository,
								IMapper mapper,
								ICompanyService companyService,
								ICurrencyService currencyService,
								IProductService productService,
								IBankService bankService,
								ICustomerService customerService,
								IUserAuthorizedDataService userAuthorizedDataService)
		{
			_repository = repository;
			_mapper = mapper;
			_companyService = companyService;
			_currencyService = currencyService;
			_productService = productService;
			_bankService = bankService;
			_customerService = customerService;
			_userAuthorizedDataService = userAuthorizedDataService;
		}

		public async Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int userId, bool isAdmin, int invoiceId)
		{
			InfoForNewInvoiceDto result = new InfoForNewInvoiceDto();

			var companies = _companyService.GetAllCompanyAutocompleteDto(userId, isAdmin);
			var currencies = _currencyService.GetCurrencyAutocompleteDto();

			await Task.WhenAll(companies, currencies);

			result.Companies = await companies;
			result.Currencies = await currencies;

			return result;
		}

		public async Task<ResponsePaged<InvoicesViewDto>> GetPagedInvoicesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var userAuthorizedCompanies = await _companyService.GetAuthorizedCompaniesAsync(queryPaged.UserId, queryPaged.IsAdmin);

			var invoices = _repository.GetInvoicesAsync(queryPaged, GetOrdering(queryPaged.SortLabel), userAuthorizedCompanies);
			var totalCount = _repository.GetCountInvoicesAsync(queryPaged, userAuthorizedCompanies);

			await Task.WhenAll(invoices, totalCount);

			return new ResponsePaged<InvoicesViewDto>
			{
				Items = _mapper.Map<IReadOnlyCollection<InvoicesViewDto>>(await invoices),
				TotalCount = await totalCount
			};
		}

		public async Task<ResponseRedirect> CreateInvoiceAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto)
		{
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			var userAuthorizedCompanies = await _companyService.GetAuthorizedCompaniesAsync(userId, isAdmin);
			if (!userAuthorizedCompanies.Any(c => c.Id == company.Id))
				throw new InvalidOperationException("Access Denied");

			var existsInvoice = new Invoice
			{
				InvoiceNumber = await _repository.GetNextInvoiceNumberForCompanyAsync(company.Id)
			};

			await MapInvoice(existsInvoice, invoiceDto, company, currency, bank, customer);

			var entityId = await _repository.CreateInvoiceAsync(existsInvoice);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<Invoice?> GetInvoiceByIdAsync(int userId, bool isAdmin, int id)
		{
			var userAuthorizedCompanies = await _companyService.GetAuthorizedCompaniesAsync(userId, isAdmin);
			return await _repository.GetInvoiceByIdAsync(id, userAuthorizedCompanies);
		}

		public async Task<InvoiceEditDto> GetInvoiceDtoByIdAsync(int userId, bool isAdmin, int id)
		{
			var invoice = await ValidateExistsEntityAsync(userId, isAdmin, id);
			return _mapper.Map<InvoiceEditDto>(invoice);
		}

		public async Task<ResponseRedirect> UpdateInvoiceAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto)
		{
			var existsInvoice = await ValidateExistsEntityAsync(userId, isAdmin, invoiceDto.Id);
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			await MapInvoice(existsInvoice, invoiceDto, company, currency, bank, customer);

			var entityId = await _repository.UpdateInvoiceAsync(existsInvoice);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<ResponseBoolean> DeleteInvoiceAsync(int userId, bool isAdmin, int id)
		{
			var invoice = await ValidateExistsEntityAsync(userId, isAdmin, id);
			return new ResponseBoolean
			{
				Result = await _repository.DeleteInvoiceAsync(invoice)
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int userId, bool isAdmin, int id, int modeDelete)
		{
			if (modeDelete == 0)
			{
				var existsEntity = await ValidateExistsEntityAsync(userId, isAdmin, id);
				existsEntity.IsDeleted = true;

				await _repository.UpdateInvoiceAsync(existsEntity);

				return new ResponseBoolean { Result = true };
			}
			return await DeleteInvoiceAsync(userId, isAdmin, id);
		}

		public async Task<ResponseBoolean> ArchiveUnarchiveEntity(QueryInvoiceChangeArchive queryArchive)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryArchive.UserId, queryArchive.IsAdmin, queryArchive.EntityId);
			existsEntity.IsArchived = queryArchive.Archive;

			var entityId = await _repository.UpdateInvoiceAsync(existsEntity);

			return new ResponseBoolean
			{
				Result = true
			};
		}


		public async Task<ResponseBoolean> ChangeInvoiceStatus(QueryInvoiceChangeStatus queryStatus)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryStatus.UserId, queryStatus.IsAdmin, queryStatus.EntityId);
			existsEntity.Status = queryStatus.Status;

			var entityId = await _repository.UpdateInvoiceAsync(existsEntity);

			return new ResponseBoolean
			{
				Result = true
			};
		}

		private async Task<Invoice> ValidateExistsEntityAsync(int userId, bool isAdmin, int id)
		{
			var userAuthorizedCompanies = await _companyService.GetAuthorizedCompaniesAsync(userId, isAdmin);
			var existsInvoice = await _repository.GetInvoiceByIdAsync(id, userAuthorizedCompanies)
							?? throw new InvalidOperationException("Invoice not found");

			return existsInvoice;
		}

		private async Task<(Currency, Company, Bank, Customer)> ValidateInputAsync(InvoiceEditDto invoiceDto)
		{
			var company = await _companyService.GetCompanyByIdAsync(invoiceDto.Company.Id)
							?? throw new InvalidOperationException($"Company: {invoiceDto.Company.Id} not found.");

			var currency = await _currencyService.GetCurrencyByIdAsync(invoiceDto.Currency.Id)
							?? throw new InvalidOperationException($"Company: {invoiceDto.Currency.Id} not found.");

			var bank = await _bankService.GetBankByIdAsync(invoiceDto.Bank.Id)
							?? throw new InvalidOperationException($"Bank: {invoiceDto.Bank.Id} not found.");

			var customer = await _customerService.GetCustomerByIdAsync(invoiceDto.Customer.Id)
							?? throw new InvalidOperationException($"Customer: {invoiceDto.Customer.Id} not found.");

			return (currency, company, bank, customer);
		}

		private decimal CalculateTotalAmount(IEnumerable<InvoiceItem> items, bool enabledVat, decimal vat)
		{
			var total = items.Sum(item => item.Price * item.Quantity);
			return enabledVat ? total + total / 100 * vat : total;
		}

		private async Task MapInvoice(Invoice existsInvoice, InvoiceEditDto dto, Company company, Currency currency, Bank bank, Customer customer)
		{
			existsInvoice.CompanyId = company.Id;
			existsInvoice.Company = company;
			existsInvoice.PONumber = dto.PONumber;
			existsInvoice.Vat = dto.Vat;
			existsInvoice.EnabledVat = dto.EnabledVat;
			existsInvoice.DateTime = dto.DateTime ?? DateTime.Now;
			existsInvoice.DueDate = dto.DueDate ?? DateTime.Now.AddDays(company.PaymentTerms);
			existsInvoice.CustomerId = customer.Id;
			existsInvoice.Customer = customer;
			existsInvoice.CurrencyId = currency.Id;
			existsInvoice.Currency = currency;
			existsInvoice.BankId = bank.Id;
			existsInvoice.Bank = bank;

			List<InvoiceItem> invoiceItem = new List<InvoiceItem>();
			foreach (var item in dto.InvoiceItems)
			{
				var product = await _productService.GetProductByIdAsync(item.Product.Id);
				invoiceItem.Add(new InvoiceItem
				{
					ProductId = product.Id,
					Product = product,
					Price = item.Price,
					Quantity = item.Quantity
				});
			}

			existsInvoice.InvoiceItems = invoiceItem;
			existsInvoice.TotalAmount = CalculateTotalAmount(existsInvoice.InvoiceItems, existsInvoice.EnabledVat, existsInvoice.Vat);
		}

		private Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> GetOrdering(string sortLabel)
		{
			var orderingOptions = new Dictionary<string, Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>>>
			{
				{"Id_desc", q => q.OrderByDescending(e => e.Id)},
				{"DateTime", q => q.OrderBy(e => e.DateTime)},
				{"DateTime_desc", q => q.OrderByDescending(e => e.DateTime)}
			};

			return orderingOptions.GetValueOrDefault(sortLabel, q => q.OrderBy(e => e.Id));
		}

	}

}
