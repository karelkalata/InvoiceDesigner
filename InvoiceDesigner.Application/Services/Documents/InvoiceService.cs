using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.InvoiceDTOs;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.Documents
{
	public class InvoiceService : IInvoiceService
	{
		private readonly IInvoiceRepository _repository;
		private readonly IMapper _mapper;
		private readonly ICompanyService _serviceCompany;
		private readonly ICurrencyService _currencyService;
		private readonly IProductService _productService;
		private readonly IBankService _bankService;
		private readonly ICustomerService _customerService;
		private readonly IAccountingService _serviceAccounting;

		public InvoiceService(IInvoiceRepository repository,
								IMapper mapper,
								ICompanyService companyService,
								ICurrencyService currencyService,
								IProductService productService,
								IBankService bankService,
								ICustomerService customerService,
								IAccountingService accountingService)
		{
			_repository = repository;
			_mapper = mapper;
			_serviceCompany = companyService;
			_currencyService = currencyService;
			_productService = productService;
			_bankService = bankService;
			_customerService = customerService;
			_serviceAccounting = accountingService;
		}

		public async Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int userId, bool isAdmin, int invoiceId)
		{
			InfoForNewInvoiceDto result = new();

			var companies = _serviceCompany.GetAllAutocompleteDto(userId, isAdmin);
			var currencies = _currencyService.GetAutocompleteDto();

			await Task.WhenAll(companies, currencies);

			result.Companies = await companies;
			result.Currencies = await currencies;

			return result;
		}


		public async Task<ResponsePaged<InvoicesViewDto>> GetPagedAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(queryPaged.UserId, queryPaged.IsAdmin);

			var invoices = _repository.GetAsync(queryPaged, GetOrdering(queryPaged.SortLabel), userAuthorizedCompanies);
			var totalCount = _repository.GetCountAsync(queryPaged, userAuthorizedCompanies);

			await Task.WhenAll(invoices, totalCount);

			return new ResponsePaged<InvoicesViewDto>
			{
				Items = _mapper.Map<IReadOnlyCollection<InvoicesViewDto>>(await invoices),
				TotalCount = await totalCount
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto)
		{
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(userId, isAdmin);
			if (!userAuthorizedCompanies.Any(c => c.Id == company.Id))
				throw new InvalidOperationException("Access Denied");

			var existsInvoice = new Invoice
			{
				Number = await _repository.GetNextNumberForCompanyAsync(company.Id)
			};

			await MapDtoToEntity(existsInvoice, invoiceDto, company, currency, bank, customer);

			var entityId = await _repository.CreateAsync(existsInvoice);

			await _serviceAccounting.CreateEntriesAsync(existsInvoice, EAccountingDocument.Invoice, existsInvoice.Status);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<Invoice?> GetByIdAsync(int userId, bool isAdmin, int id)
		{
			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(userId, isAdmin);
			return await _repository.GetByIdAsync(id, userAuthorizedCompanies);
		}

		public async Task<InvoiceEditDto> GetDtoByIdAsync(int userId, bool isAdmin, int id)
		{
			var invoice = await ValidateExistsEntityAsync(userId, isAdmin, id);
			return _mapper.Map<InvoiceEditDto>(invoice);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto)
		{
			var existsEntity = await ValidateExistsEntityAsync(userId, isAdmin, invoiceDto.Id);
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			await MapDtoToEntity(existsEntity, invoiceDto, company, currency, bank, customer);

			var entityId = await _repository.UpdateAsync(existsEntity);

			await _serviceAccounting.CreateEntriesAsync(existsEntity, EAccountingDocument.Invoice, existsEntity.Status);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = entityId
			};
		}

		public async Task<ResponseBoolean> DeleteAsync(int userId, bool isAdmin, int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(userId, isAdmin, id);
			await _serviceAccounting.CreateEntriesAsync(existsEntity, EAccountingDocument.Invoice, EStatus.Canceled);

			return new ResponseBoolean
			{
				Result = await _repository.DeleteAsync(existsEntity)
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int userId, bool isAdmin, int id, int modeDelete)
		{
			var existsEntity = await ValidateExistsEntityAsync(userId, isAdmin, id);

			if (modeDelete == 0)
			{
				existsEntity.IsDeleted = true;
				await _repository.UpdateAsync(existsEntity);
				await _serviceAccounting.CreateEntriesAsync(existsEntity, EAccountingDocument.Invoice, EStatus.Canceled);

				return new ResponseBoolean
				{
					Result = true
				};
			}

			return await DeleteAsync(userId, isAdmin, id);
		}

		public async Task<ResponseBoolean> OnChangeProperty(QueryOnChangeProperty queryStatus)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryStatus.UserId, queryStatus.IsAdmin, queryStatus.EntityId);

			// If the document has the status delete - then cancel all double entries in the ledger
			if (queryStatus.IsDeleted) 
				existsEntity.Status = EStatus.Canceled;
			else
				existsEntity.Status = queryStatus.Status;

			existsEntity.IsArchived = queryStatus.IsArchived;
			existsEntity.IsDeleted = queryStatus.IsDeleted;

			await _repository.UpdateAsync(existsEntity);
			await _serviceAccounting.CreateEntriesAsync(existsEntity, EAccountingDocument.Invoice, existsEntity.Status);
			return new ResponseBoolean
			{
				Result = true
			};
		}

		private async Task<Invoice> ValidateExistsEntityAsync(int userId, bool isAdmin, int id)
		{
			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(userId, isAdmin);
			var existsInvoice = await _repository.GetByIdAsync(id, userAuthorizedCompanies)
							?? throw new InvalidOperationException("Invoice not found");

			return existsInvoice;
		}

		private async Task<(Currency, Company, Bank, Customer)> ValidateInputAsync(InvoiceEditDto invoiceDto)
		{
			var company = await _serviceCompany.GetByIdAsync(invoiceDto.Company.Id)
							?? throw new InvalidOperationException($"Company: {invoiceDto.Company.Id} not found.");

			var currency = await _currencyService.GetByIdAsync(invoiceDto.Currency.Id)
							?? throw new InvalidOperationException($"Company: {invoiceDto.Currency.Id} not found.");

			var bank = await _bankService.GetByIdAsync(invoiceDto.Bank.Id)
							?? throw new InvalidOperationException($"Bank: {invoiceDto.Bank.Id} not found.");

			var customer = await _customerService.GetByIdAsync(invoiceDto.Customer.Id)
							?? throw new InvalidOperationException($"CustomerId: {invoiceDto.Customer.Id} not found.");

			return (currency, company, bank, customer);
		}

		private static decimal CalculateTotalAmount(IEnumerable<InvoiceItem> items, bool enabledVat, decimal vat)
		{
			var total = items.Sum(item => item.Price * item.Quantity);
			return enabledVat ? total + total / 100 * vat : total;
		}

		private async Task MapDtoToEntity(Invoice existsEntity, InvoiceEditDto dto, Company company, Currency currency, Bank bank, Customer customer)
		{
			existsEntity.CompanyId = company.Id;
			existsEntity.Company = company;
			existsEntity.PONumber = dto.PONumber;
			existsEntity.Vat = dto.Vat;
			existsEntity.EnabledVat = dto.EnabledVat;
			existsEntity.DateTime = dto.DateTime ?? DateTime.UtcNow;
			existsEntity.DueDate = dto.DueDate ?? DateTime.UtcNow.AddDays(company.PaymentTerms);
			
			existsEntity.CustomerId = customer.Id;
			existsEntity.Customer = customer;
			existsEntity.CurrencyId = currency.Id;
			existsEntity.Currency = currency;
			existsEntity.BankId = bank.Id;
			existsEntity.Bank = bank;

			// If the document has the status delete - then cancel all double entries in the ledger
			if (existsEntity.IsDeleted)
				existsEntity.Status = EStatus.Canceled;
			else
				existsEntity.Status = dto.Status;

			List<InvoiceItem> invoiceItem = new();
			foreach (var item in dto.InvoiceItems)
			{
				var product = await _productService.GetByIdAsync(item.Item.Id);
				invoiceItem.Add(new InvoiceItem
				{
					ItemId = product.Id,
					Item = product,
					Price = item.Price,
					Quantity = item.Quantity
				});
			}

			existsEntity.InvoiceItems = invoiceItem;
			existsEntity.Amount = CalculateTotalAmount(existsEntity.InvoiceItems, existsEntity.EnabledVat, existsEntity.Vat);
		}

		private static Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> GetOrdering(string sortLabel)
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
