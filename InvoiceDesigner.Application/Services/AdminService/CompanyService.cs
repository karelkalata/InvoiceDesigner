using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Company;
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
	public class CompanyService : ABaseService<Company>, ICompanyService
	{
		private readonly ICompanyRepository _repository;
		private readonly IInvoiceRepository _repositoryInvoice;
		private readonly ICurrencyRepository _repositoryCurrency;
		private readonly IUserRepository _repositoryUser;

		public CompanyService(ICompanyRepository repository, IInvoiceRepository invoiceRepository, ICurrencyRepository currencyRepository, IUserRepository repositoryUser) : base(repository)
		{
			_repository = repository;
			_repositoryInvoice = invoiceRepository;
			_repositoryCurrency = currencyRepository;
			_repositoryUser = repositoryUser;
		}

		public async Task<ResponsePaged<CompanyViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand)
		{
			var (companies, total) = await GetEntitiesAndCountAsync(pagedCommand);

			return new ResponsePaged<CompanyViewDto>
			{
				Items = CompanyMapper.ToViewDto(companies),
				TotalCount = total
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, CompanyEditDto companyEditDto)
		{
			var currency = await ValidateInputAsync(companyEditDto);

			var company = new Company();
			await MapCompany(company, companyEditDto, currency);

			await _repository.CreateAsync(company);

			return new ResponseRedirect
			{
				RedirectUrl = "/Companies"
			};
		}

		public async Task<Company> GetByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<CompanyEditDto> GetEditDtoByIdAsync(int id)
		{
			var company = await ValidateExistsEntityAsync(id);
			return CompanyMapper.ToEditDto(company);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, CompanyEditDto companyEditDto)
		{
			var existingCompanyTask = ValidateExistsEntityAsync(companyEditDto.Id);
			var currencyTask = ValidateInputAsync(companyEditDto);

			await Task.WhenAll(existingCompanyTask, currencyTask);

			var existingCompany = await existingCompanyTask;
			var currency = await currencyTask;

			await MapCompany(existingCompany, companyEditDto, currency);

			await _repository.UpdateAsync(existingCompany);

			return new ResponseRedirect
			{
				RedirectUrl = "/Companies"
			};
		}


		public async Task<int> GetCountAsync() => await _repository.GetCountAsync(new GetCountFilter());

		public async Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin)
		{
			if (isAdmin)
			{
				var pagedFilter = new PagedFilter
				{
					PageSize = 9999999,
					Page = 0,
				};
				return await _repository.GetEntitiesAsync(pagedFilter);
			}
			else
			{
				var user = await _repositoryUser.GetByIdAsync(new GetByIdFilter { Id = userId });
				if (user != null)
					return user.Companies.ToList();
			}
			return new List<Company>();
		}

		public async Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllAutocompleteDto(int userId, bool isAdmin)
		{
			var companies = await GetAuthorizedCompaniesAsync(userId, isAdmin);
			return CompanyMapper.ToAutocompleteDto(companies);
		}

		public async Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string searchText)
		{
			var pagedFilter = new PagedFilter
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText,
				SortLabel = "Name"
			};

			var companies = await _repository.GetEntitiesAsync(pagedFilter);
			return CompanyMapper.ToAutocompleteDto(companies);
		}

		private async Task<Company> ValidateExistsEntityAsync(int id)
		{
			var existsCompany = await _repository.GetByIdAsync(new GetByIdFilter { Id = id })
							?? throw new InvalidOperationException("Company not found");
			return existsCompany;
		}

		private async Task<Currency> ValidateInputAsync(CompanyEditDto dto)
		{
			var currency = await _repositoryCurrency.GetByIdAsync(new GetByIdFilter { Id = dto.Currency.Id })
							?? throw new InvalidOperationException($"Currency with ID {dto.Currency.Id} not found.");
			return currency;
		}

		private async Task<Company> MapCompany(Company company, CompanyEditDto dto, Currency currency)
		{
			company.Name = dto.Name;
			company.TaxId = dto.TaxId;
			company.VatId = dto.VatId;
			company.WWW = dto.WWW?.Trim();
			company.Email = dto.Email?.Trim();
			company.Phone = dto.Phone?.Trim();
			company.Address = dto.Address?.Trim();
			company.Info = dto.Info?.Trim();
			company.PaymentTerms = dto.PaymentTerms;
			company.DefaultVat = dto.DefaultVat;
			company.CurrencyId = currency.Id;
			company.Currency = currency;

			var itemsBanks = new List<Bank>();

			foreach (var item in dto.Banks)
			{
				var existsBank = company.Banks.FirstOrDefault(e => e.Id == item.Id);
				if (existsBank == null)
				{
					existsBank = new Bank
					{
						CompanyId = company.Id,
						Company = company
					};

				}
				var bankCurrency = await _repositoryCurrency.GetByIdAsync(new GetByIdFilter { Id = item.Currency.Id })
									?? throw new InvalidOperationException($"Currency with ID {item.Currency.Id} not found.");

				existsBank.CurrencyId = bankCurrency.Id;
				existsBank.Currency = bankCurrency;
				existsBank.Name = item.Name.Trim();
				existsBank.BIC = item.BIC.Trim();
				existsBank.Account = item.Account.Trim();
				existsBank.Address = item.Address.Trim();

				itemsBanks.Add(existsBank);
			}
			company.Banks = itemsBanks;
			return company;
		}
	}
}
