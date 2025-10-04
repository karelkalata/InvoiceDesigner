using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs;
using InvoiceDesigner.Application.DTOs.Invoice;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Application.Mapper;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Application.Services.Abstract;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Services.Documents
{
	public class InvoiceService : ABaseService<Invoice>, IInvoiceService
	{
		private readonly IInvoiceRepository _repository;
		private readonly ICompanyRepository _repositoryCompany;
		private readonly IProductRepository _repositoryProduct;
		private readonly ICurrencyRepository _repositoryCurrency;
		private readonly IBankRepository _repositoryBank;
		private readonly ICustomerRepository _repositoryCustomer;
		private readonly IUserRepository _repositoryUser;

		public InvoiceService(IInvoiceRepository repository,
								ICompanyRepository repositoryCompany,
								IProductRepository repositoryProduct,
								ICurrencyRepository repositoryCurrency,
								IBankRepository repositoryBank,
								ICustomerRepository epositoryCustomer,
								IUserRepository repositoryUser
			) : base(repository)
		{
			_repository = repository;
			_repositoryCompany = repositoryCompany;
			_repositoryProduct = repositoryProduct;
			_repositoryCurrency = repositoryCurrency;
			_repositoryBank = repositoryBank;
			_repositoryCustomer = epositoryCustomer;
			_repositoryUser = repositoryUser;
		}

		public override async Task<(IReadOnlyCollection<Invoice> Entities, int TotalCount)> GetEntitiesAndCountAsync(PagedCommand pagedCommand)
		{

			var pagedFilter = new PagedFilter
			{
				PageSize = pagedCommand.PageSize,
				Page = pagedCommand.Page,
				ShowDeleted = pagedCommand.ShowDeleted,
				ShowArchived = pagedCommand.ShowArchived,
				SearchString = pagedCommand.SearchString,
				SortLabel = pagedCommand.SortLabel,
				UserAuthorizedCompanies = await GetAuthorizedCompaniesAsync(pagedCommand.UserId, pagedCommand.IsAdmin),
			};

			var entitiesTask = _repository.GetEntitiesAsync(pagedFilter);
			var totalCountTask = _repository.GetCountAsync(new GetCountFilter
			{
				ShowDeleted = pagedCommand.ShowDeleted,
				ShowArchived = pagedCommand.ShowArchived
			});

			await Task.WhenAll(entitiesTask, totalCountTask);

			return (await entitiesTask, await totalCountTask);
		}

		public async Task<ResponsePaged<InvoicesViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand)
		{
			var (entities, total) = await GetEntitiesAndCountAsync(pagedCommand);
			var dtos = InvoiceMapper.ToViewDto(entities);

			return new ResponsePaged<InvoicesViewDto>
			{
				Items = dtos,
				TotalCount = total
			};
		}



		public async Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int userId, bool isAdmin, int invoiceId)
		{
			var companies = await GetAuthorizedCompaniesAsync(userId, isAdmin);
			var currencies = await _repositoryCurrency.GetAllAsync();

			return new InfoForNewInvoiceDto
			{
				Companies = CompanyMapper.ToAutocompleteDto(companies),
				Currencies = CurrencyMapper.ToAutocompleteDto(currencies)
			};
		}




		public async Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto)
		{
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			var userAuthorizedCompanies = await GetAuthorizedCompaniesAsync(userId, isAdmin);
			if (!userAuthorizedCompanies.Any(c => c.Id == company.Id))
				throw new InvalidOperationException("Access Denied");

			var existsInvoice = new Invoice
			{
				Number = await _repository.GetNextNumberForCompanyAsync(company.Id)
			};

			await MapDtoToEntity(existsInvoice, invoiceDto, company, currency, bank, customer);
			await _repository.CreateAsync(existsInvoice);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = existsInvoice.Id
			};
		}

		public async Task<Invoice?> GetByIdAsync(int userId, bool isAdmin, int id)
		{
			var userAuthorizedCompanies = await GetAuthorizedCompaniesAsync(userId, isAdmin);
			var getByIdFilter = new GetByIdFilter
			{
				Id = id,
				userAuthorizedCompanies = userAuthorizedCompanies
			};
			return await _repository.GetByIdAsync(getByIdFilter);
		}

		public async Task<InvoiceEditDto> GetDtoByIdAsync(int userId, bool isAdmin, int id)
		{
			var invoice = await ValidateExistsEntityAsync(userId, isAdmin, id);
			return InvoiceMapper.ToEditDto(invoice);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto)
		{
			var existsEntity = await ValidateExistsEntityAsync(userId, isAdmin, invoiceDto.Id);
			var (currency, company, bank, customer) = await ValidateInputAsync(invoiceDto);

			await MapDtoToEntity(existsEntity, invoiceDto, company, currency, bank, customer);

			await _repository.UpdateAsync(existsEntity);

			return new ResponseRedirect
			{
				RedirectUrl = string.Empty,
				entityId = existsEntity.Id
			};
		}

		public async Task<ResponseBoolean> DeleteAsync(int userId, bool isAdmin, int id)
		{
			var existsEntity = await ValidateExistsEntityAsync(userId, isAdmin, id);

			return new ResponseBoolean
			{
				Result = await _repository.DeleteAsync(existsEntity)
			};
		}


		public async Task<ResponseBoolean> OnChangeProperty(ChangePropertyCommand changePropertyCommand)
		{
			var existsEntity = await ValidateExistsEntityAsync(changePropertyCommand.UserId, changePropertyCommand.IsAdmin, changePropertyCommand.EntityId);

			// If the document has the status delete - then cancel all double entries in the ledger
			if (changePropertyCommand.IsDeleted)
				existsEntity.Status = EStatus.Canceled;
			else
				existsEntity.Status = changePropertyCommand.Status;

			existsEntity.IsArchived = changePropertyCommand.IsArchived;
			existsEntity.IsDeleted = changePropertyCommand.IsDeleted;

			await _repository.UpdateAsync(existsEntity);
			return new ResponseBoolean
			{
				Result = true
			};
		}

		private async Task<Invoice> ValidateExistsEntityAsync(int userId, bool isAdmin, int id)
		{
			var getByIdFilter = new GetByIdFilter
			{
				Id = id,
				userAuthorizedCompanies = await GetAuthorizedCompaniesAsync(userId, isAdmin)
			};
			var existsInvoice = await _repository.GetByIdAsync(getByIdFilter)
							?? throw new InvalidOperationException("Invoice not found");

			return existsInvoice;
		}

		private async Task<(Currency, Company, Bank, Customer)> ValidateInputAsync(InvoiceEditDto invoiceDto)
		{
			var company = await _repositoryCompany.GetByIdAsync(new GetByIdFilter { Id = invoiceDto.Company.Id })
							?? throw new InvalidOperationException($"Company: {invoiceDto.Company.Id} not found.");

			var currency = await _repositoryCurrency.GetByIdAsync(new GetByIdFilter { Id = invoiceDto.Currency.Id })
							?? throw new InvalidOperationException($"Company: {invoiceDto.Currency.Id} not found.");

			var bank = await _repositoryBank.GetByIdAsync(new GetByIdFilter { Id = invoiceDto.Bank.Id })
							?? throw new InvalidOperationException($"Bank: {invoiceDto.Bank.Id} not found.");

			var customer = await _repositoryCustomer.GetByIdAsync(new GetByIdFilter { Id = invoiceDto.Customer.Id })
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
				var product = await _repositoryProduct.GetByIdAsync(new GetByIdFilter { Id = item.Item.Id })
						?? throw new InvalidOperationException($"Product with ID {item.Item.Id} not found.");
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

		private async Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin)
		{
			if (isAdmin)
			{
				return await _repositoryCompany.GetAllCompanies();
			}
			else
			{
				var user = await _repositoryUser.GetByIdAsync(new GetByIdFilter { Id = userId });
				if (user != null)
					return user.Companies.ToList();
			}
			return new List<Company>();
		}
	}

}
