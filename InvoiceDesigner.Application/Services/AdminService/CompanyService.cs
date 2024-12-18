﻿using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.AdminService
{
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyRepository _repoCompany;
		private readonly IMapper _mapper;
		private readonly ICurrencyService _currencyService;
		private readonly IInvoiceServiceHelper _invoiceServiceHelper;
		private readonly IUserAuthorizedDataService _userAuthorizedData;
		private readonly IUserActivityLogService _userActivity;

		public CompanyService(ICompanyRepository repoCompany,
								IMapper mapper,
								ICurrencyService currencyService,
								IInvoiceServiceHelper invoiceServiceHelper,
								IUserAuthorizedDataService userAuthorizedData,
								IUserActivityLogService userActivity)
		{
			_repoCompany = repoCompany;
			_mapper = mapper;
			_currencyService = currencyService;
			_invoiceServiceHelper = invoiceServiceHelper;
			_userAuthorizedData = userAuthorizedData;
			_userActivity = userActivity;
		}

		public async Task<ResponsePaged<CompanyViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var companiesTask = _repoCompany.GetEntitiesAsync(queryPaged, GetOrdering(queryPaged.SortLabel));
			var totalCountTask = _repoCompany.GetCountAsync();

			await Task.WhenAll(companiesTask, totalCountTask);

			return new ResponsePaged<CompanyViewDto>
			{
				Items = _mapper.Map<IReadOnlyCollection<CompanyViewDto>>(await companiesTask),
				TotalCount = await totalCountTask
			};
		}

		public async Task<ResponseRedirect> CreateAsync(int userId, CompanyEditDto companyEditDto)
		{
			var currency = await ValidateInputAsync(companyEditDto);

			var company = new Company();
			await MapCompany(company, companyEditDto, currency);

			var entityId = await _repoCompany.CreateAsync(company);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.Company, EActivitiesType.Create, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = "/Companies"
			};
		}

		public async Task<Company> GetByIdAsync(int id) => await ValidateExistsEntityAsync(id);

		public async Task<CompanyEditDto> GetEditDtoByIdAsync(int id)
		{
			var company = await ValidateExistsEntityAsync(id);
			return _mapper.Map<CompanyEditDto>(company);
		}

		public async Task<ResponseRedirect> UpdateAsync(int userId, CompanyEditDto companyEditDto)
		{
			var existingCompanyTask = ValidateExistsEntityAsync(companyEditDto.Id);
			var currencyTask = ValidateInputAsync(companyEditDto);

			await Task.WhenAll(existingCompanyTask, currencyTask);

			var existingCompany = await existingCompanyTask;
			var currency = await currencyTask;

			await MapCompany(existingCompany, companyEditDto, currency);

			var entityId = await _repoCompany.UpdateAsync(existingCompany);
			await _userActivity.CreateActivityLog(userId, EDocumentsTypes.Company, EActivitiesType.Update, entityId);

			return new ResponseRedirect
			{
				RedirectUrl = "/Companies"
			};
		}

		public async Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity)
		{
			var existsEntity = await ValidateExistsEntityAsync(queryDeleteEntity.EntityId);

			if (!queryDeleteEntity.MarkAsDeleted)
			{
				if (await _invoiceServiceHelper.IsCompanyUsedInInvoices(queryDeleteEntity.EntityId))
					throw new InvalidOperationException($"{existsEntity.Name} is in use in Invoices and cannot be deleted.");

				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.Company, EActivitiesType.Delete, existsEntity.Id);

				return new ResponseBoolean
				{
					Result = await _repoCompany.DeleteAsync(existsEntity)
				};
			}
			else
			{
				existsEntity.IsDeleted = true;
				await _repoCompany.UpdateAsync(existsEntity);
				await _userActivity.CreateActivityLog(queryDeleteEntity.UserId, EDocumentsTypes.Company, EActivitiesType.MarkedAsDeleted, existsEntity.Id);

				return new ResponseBoolean
				{
					Result = true
				};
			}
		}

		public async Task<int> GetCountAsync() => await _repoCompany.GetCountAsync();

		public async Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin)
		{
			var companies = isAdmin
				? await _repoCompany.GetAllCompaniesDto()
				: await _userAuthorizedData.GetAuthorizedCompaniesAsync(userId);

			return companies.ToList();
		}

		public async Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllAutocompleteDto(int userId, bool isAdmin)
		{
			return _mapper.Map<IReadOnlyCollection<CompanyAutocompleteDto>>(await GetAuthorizedCompaniesAsync(userId, isAdmin));
		}

		public async Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string searchText)
		{
			var queryPaged = new QueryPaged
			{
				PageSize = 10,
				Page = 1,
				SearchString = searchText
			};

			var companies = await _repoCompany.GetEntitiesAsync(queryPaged, GetOrdering("Value"));
			return _mapper.Map<IReadOnlyCollection<CompanyAutocompleteDto>>(companies);
		}

		private async Task<Company> ValidateExistsEntityAsync(int id)
		{
			var existsCompany = await _repoCompany.GetByIdAsync(id)
							?? throw new InvalidOperationException("Company not found");
			return existsCompany;
		}

		private async Task<Currency> ValidateInputAsync(CompanyEditDto dto)
		{
			var currency = await _currencyService.GetByIdAsync(dto.Currency.Id)
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
				var bankCurrency = await _currencyService.GetByIdAsync(item.Currency.Id)
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

		private Func<IQueryable<Company>, IOrderedQueryable<Company>> GetOrdering(string sortLabel)
		{
			var sortingOptions = new Dictionary<string, Func<IQueryable<Company>, IOrderedQueryable<Company>>>
			{
				{"Id_desc", q => q.OrderByDescending(c => c.Id)},
				{"TaxId", q => q.OrderBy(c => c.TaxId)},
				{"TaxId_desc", q => q.OrderByDescending(c => c.TaxId)},
				{"Value", q => q.OrderBy(c => c.Name)},
				{"Name_desc", q => q.OrderByDescending(c => c.Name)},
				{"Id", q => q.OrderBy(c => c.Id)}
			};

			return sortingOptions.TryGetValue(sortLabel, out var orderFunc) ? orderFunc : sortingOptions["Id"];
		}


	}

}
