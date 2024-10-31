using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;

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

		public InvoiceService(IInvoiceRepository repository,
								IMapper mapper,
								ICompanyService companyService,
								ICurrencyService currencyService,
								IProductService productService,
								IBankService bankService,
								ICustomerService customerService)
		{
			_repository = repository;
			_mapper = mapper;
			_companyService = companyService;
			_currencyService = currencyService;
			_productService = productService;
			_bankService = bankService;
			_customerService = customerService;

		}

		public async Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int invoiceId)
		{
			InfoForNewInvoiceDto result = new InfoForNewInvoiceDto();

			var companies = _companyService.GetAllCompanyAutocompleteDto();
			var banks = _bankService.GetAllBanksAutocompleteDto();
			var currencies = _currencyService.GetCurrencyAutocompleteDto();

			await Task.WhenAll(companies, banks, currencies);

			result.Companies = await companies;
			result.Banks = await banks;
			result.Currencies = await currencies;

			return result;
		}

		public async Task<PagedResult<InvoicesViewDto>> GetPagedInvoicesAsync(int pageSize, int page, string searchString, string sortLabel)
		{
			pageSize = Math.Max(pageSize, 1);
			page = Math.Max(page, 1);

			var invoices = _repository.GetInvoicesAsync(pageSize, page, searchString, GetOrdering(sortLabel));
			var totalCount = _repository.GetCountInvoicesAsync();

			await Task.WhenAll(invoices, totalCount);

			return new PagedResult<InvoicesViewDto>
			{
				Items = _mapper.Map<IReadOnlyCollection<InvoicesViewDto>>(await invoices),
				TotalCount = await totalCount
			};
		}

		public async Task<ResponseRedirect> CreateInvoiceAsync(InvoiceEditDto invoiceDto)
		{
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);
			var existsInvoice = new Invoice
			{
				InvoiceNumber = await _repository.GetNextInvoiceNumberForCompanyAsync(company.Id)
			};

			await MapInvoice(existsInvoice, invoiceDto, company, currency, bank, customer);

			var entityId = await _repository.CreateInvoiceAsync(existsInvoice);

			return new ResponseRedirect
			{
				RedirectUrl = $"/Invoices/Edit/{entityId}"
			};
		}

		public async Task<Invoice?> GetInvoiceByIdAsync(int id)
		{
			return await _repository.GetInvoiceByIdAsync(id);
		}

		public async Task<InvoiceEditDto> GetInvoiceDtoByIdAsync(int id)
		{
			var invoice = await ValidateExistsInvoiceAsync(id);
			return _mapper.Map<InvoiceEditDto>(invoice);
		}

		public async Task<ResponseRedirect> UpdateInvoiceAsync(InvoiceEditDto invoiceDto)
		{
			var existsInvoice = await ValidateExistsInvoiceAsync(invoiceDto.Id);
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			await MapInvoice(existsInvoice, invoiceDto, company, currency, bank, customer);

			var entityId = await _repository.UpdateInvoiceAsync(existsInvoice);

			return new ResponseRedirect
			{
				RedirectUrl = $"/Invoices/Edit/{entityId}"
			};
		}

		public async Task<bool> DeleteInvoiceAsync(int id)
		{
			var invoice = await ValidateExistsInvoiceAsync(id);
			return await _repository.DeleteInvoiceAsync(invoice);
		}

		private async Task<Invoice> ValidateExistsInvoiceAsync(int id)
		{
			var existsInvoice = await _repository.GetInvoiceByIdAsync(id)
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

		public async Task<int> GetInvoiceCountAsync() => await _repository.GetCountInvoicesAsync();

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
