using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Services
{
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyRepository _repository;
		private readonly IMapper _mapper;
		private readonly ICurrencyService _currencyService;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;

		public CompanyService(ICompanyRepository repository,
								IMapper mapper,
								ICurrencyService currencyService,
								IInvoiceServiceHelper invoiceServiceHelper)
		{
			_repository = repository;
			_mapper = mapper;
			_currencyService = currencyService;
			_invoiceServiceHelper = invoiceServiceHelper;
		}

		public async Task<PagedResult<CompanyViewDto>> GetPagedCompaniesAsync(int pageSize, int page, string searchString, string sortLabel)
		{
			pageSize = pageSize > 0 ? pageSize : await _repository.GetCountCompaniesAsync();
			page = Math.Max(page, 1);

			var companiesTask = _repository.GetCompaniesAsync(pageSize, page, searchString, GetOrdering(sortLabel));
			var totalCountTask = _repository.GetCountCompaniesAsync();

			await Task.WhenAll(companiesTask, totalCountTask);

			return new PagedResult<CompanyViewDto>
			{
				Items = _mapper.Map<IReadOnlyCollection<CompanyViewDto>>(await companiesTask),
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateCompanyAsync(CompanyEditDto companyEditDto)
		{
			var currency = await ValidateInputAsync(companyEditDto);

			var company = new Company();
			MapCompany(company, companyEditDto, currency);

			var entityId = await _repository.CreateCompanyAsync(company);

			return new ResponseRedirect
			{
				RedirectUrl = "/Companies"
			};
		}

		public async Task<Company> GetCompanyByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<CompanyEditDto> GetCompanyEditDtoByIdAsync(int id)
		{
			var company = await ValidateExistsEntityAsync(id);
			return _mapper.Map<CompanyEditDto>(company);
		}

		public async Task<ResponseRedirect> UpdateCompanyAsync(CompanyEditDto companyEditDto)
		{
			var existingCompanyTask = ValidateExistsEntityAsync(companyEditDto.Id);
			var currencyTask = ValidateInputAsync(companyEditDto);

			await Task.WhenAll(existingCompanyTask, currencyTask);

			var existingCompany = await existingCompanyTask;
			var currency = await currencyTask;

			MapCompany(existingCompany, companyEditDto, currency);

			var entityId = await _repository.UpdateCompanyAsync(existingCompany);

			return new ResponseRedirect
			{
				RedirectUrl = "/Companies"
			};
		}


		public async Task<bool> DeleteCompanyAsync(int id)
		{
			var company = await ValidateExistsEntityAsync(id);

			if (await _invoiceServiceHelper.IsCompanyUsedInInvoices(id))
				throw new InvalidOperationException($"Company {company.Name} is in use in invoices and cannot be deleted.");

			return await _repository.DeleteCompanyAsync(company);
		}

		public async Task<int> GetCompaniesCountAsync() => await _repository.GetCountCompaniesAsync();

		public async Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllCompanyAutocompleteDto()
		{
			var companies = await _repository.GetAllCompaniesDto();
			return _mapper.Map<IReadOnlyCollection<CompanyAutocompleteDto>>(companies);
		}

		public async Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string filter)
		{
			var companies = await _repository.GetCompaniesAsync(10, 1, filter, GetOrdering("Name"));
			return _mapper.Map<IReadOnlyCollection<CompanyAutocompleteDto>>(companies);
		}

		private async Task<Company> ValidateExistsEntityAsync(int id)
		{
			var existsCompany = await _repository.GetCompanyByIdAsync(id)
							?? throw new InvalidOperationException("Company not found");
			return existsCompany;
		}

		private async Task<Currency> ValidateInputAsync(CompanyEditDto dto)
		{
			var currency = await _currencyService.GetCurrencyByIdAsync(dto.Currency.Id)
							?? throw new InvalidOperationException($"Currency with ID {dto.Currency.Id} not found.");
			return currency;
		}

		private Company MapCompany(Company company, CompanyEditDto dto, Currency currency)
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

			return company;
		}

		private Func<IQueryable<Company>, IOrderedQueryable<Company>> GetOrdering(string sortLabel)
		{
			var sortingOptions = new Dictionary<string, Func<IQueryable<Company>, IOrderedQueryable<Company>>>
			{
				{"Id_desc", q => q.OrderByDescending(c => c.Id)},
				{"TaxId", q => q.OrderBy(c => c.TaxId)},
				{"TaxId_desc", q => q.OrderByDescending(c => c.TaxId)},
				{"Name", q => q.OrderBy(c => c.Name)},
				{"Name_desc", q => q.OrderByDescending(c => c.Name)},
				{"Id", q => q.OrderBy(c => c.Id)}
			};

			return sortingOptions.TryGetValue(sortLabel, out var orderFunc) ? orderFunc : sortingOptions["Id"];
		}
	}

}
