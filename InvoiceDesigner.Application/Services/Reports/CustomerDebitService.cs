using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Application.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.DTOs.Reports.CustomerDebit;
using InvoiceDesigner.Domain.Shared.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.Reports
{
	public class CustomerDebitService : ICustomerDebitService
	{
		private readonly int ChartOfAccountsId = 1; // see Data/ChartOfAccountsData.json  "Id": 1, "Code": 1200, "Name": "Accounts Receivable",

		private readonly ICustomerDebitRepository _repoCustomerDebit;
		private readonly ICompanyService _serviceCompany;
		private readonly ICurrencyService _serviceCurrency;
		private readonly ICustomerService _serviceCustomer;
		private readonly IChartOfAccountsService _serviceChartOfAccounts;
		private Dictionary<int, string> _cachedCurrencyName = new Dictionary<int, string>();
		private Dictionary<int, string> _cachedCustomeryName = new Dictionary<int, string>();

		public CustomerDebitService(ICustomerDebitRepository repository,
									ICompanyService companyService,
									ICurrencyService currencyService,
									ICustomerService customerService,
									IChartOfAccountsService chartOfAccountsService)
		{
			_repoCustomerDebit = repository;
			_serviceCompany = companyService;
			_serviceCurrency = currencyService;
			_serviceCustomer = customerService;
			_serviceChartOfAccounts = chartOfAccountsService;
		}

		public async Task<ResponsePaged<ReportCustomerDebit>> GetPagedAsync(QueryCustomerDebit queryPaged)
		{
			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(queryPaged.UserId, queryPaged.IsAdmin);
			var userAuthorizedCompaniesIDs = userAuthorizedCompanies.Select(x => x.Id).ToList();
			var chartOfAccounts = await _serviceChartOfAccounts.ValidateExistsEntityAsync(ChartOfAccountsId);

			queryPaged.CompaniesIDs = queryPaged.CompaniesIDs
									.Where(companyId => userAuthorizedCompaniesIDs.Contains(companyId))
									.ToList();

			var entries = await _repoCustomerDebit.GetAsync(queryPaged, chartOfAccounts);
			var groupedEntries = entries
				.GroupBy(e => new { e.CustomerId, e.CurrencyId })
				.Select(g => new
				{
					CustomerId = g.Key.CustomerId,
					CurrencyId = g.Key.CurrencyId,
					TotalDebit = g.Where(e => e.IsDebit).Sum(e => e.Amount),
					TotalCredit = g.Where(e => !e.IsDebit).Sum(e => e.Amount)
				})
				.ToList();

			var reportItemsTasks = groupedEntries.Where(item => item.TotalDebit - item.TotalCredit != 0)
			   .Select(async item => new ReportCustomerDebit
			   {
				   CustomerName = await GetCustomerName(item.CustomerId),
				   CurrencyName = await GetCurrencyName(item.CurrencyId),
				   Amount = item.TotalDebit - item.TotalCredit
			   });

			var result = await Task.WhenAll(reportItemsTasks);

			return new ResponsePaged<ReportCustomerDebit>
			{
				Items = result.ToList(),
				TotalCount = result.Count()
			};
		}

		private async Task<string> GetCurrencyName(int currencyId)
		{
			if (_cachedCurrencyName.TryGetValue(currencyId, out string? currencyName))
				return currencyName;

			var name = await _serviceCurrency.GetNameByIdAsync(currencyId);
			if (name != null)
			{
				_cachedCurrencyName[currencyId] = name;
				return name;
			}

			return currencyId.ToString();
		}

		private async Task<string> GetCustomerName(int customerId)
		{
			if (_cachedCustomeryName.TryGetValue(customerId, out string? customerName))
				return customerName;

			var customer = await _serviceCustomer.GetByIdAsync(customerId);
			if (customer != null)
			{
				_cachedCustomeryName[customerId] = customer.Name;
				return customer.Name;
			}

			return customerId.ToString();
		}
	}
}
