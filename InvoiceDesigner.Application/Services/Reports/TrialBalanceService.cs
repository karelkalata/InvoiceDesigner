using InvoiceDesigner.Application.Helpers.Accounting;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.DTOs.Reports;
using InvoiceDesigner.Domain.Shared.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services.Reports
{
	public class TrialBalanceService : ITrialBalanceService
	{
		private readonly ITrialBalanceRepository _repoTrialBalance;
		private readonly ICompanyService _serviceCompany;
		private readonly ICurrencyService _serviceCurrency;
		private Dictionary<int, string> _cachedCurrencyName = new Dictionary<int, string>();


		public TrialBalanceService(ITrialBalanceRepository repository,
									ICompanyService companyService,
									ICurrencyService currencyService)
		{
			_repoTrialBalance = repository;
			_serviceCompany = companyService;
			_serviceCurrency = currencyService;
		}

		public async Task<ResponsePaged<ReportTrialBalance>> GetPagedAsync(QueryTrialBalance queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			if (queryPaged.DateStart.HasValue)
			{
				queryPaged.DateStart = queryPaged.DateStart.Value.Date;
			}

			if (queryPaged.DateEnd.HasValue)
			{
				queryPaged.DateEnd = queryPaged.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
			}

			var userAuthorizedCompanies = await _serviceCompany.GetAuthorizedCompaniesAsync(queryPaged.UserId, queryPaged.IsAdmin);
			var userAuthorizedCompaniesIDs = userAuthorizedCompanies.Select(x => x.Id).ToList();

			var queryTrialBalance = new QueryTrialBalance
			{
				PageSize = queryPaged.PageSize,
				Page = queryPaged.Page,
				DateStart = queryPaged.DateStart,
				DateEnd = queryPaged.DateEnd,
				CompaniesIDs = queryPaged.CompaniesIDs
									.Where(companyId => userAuthorizedCompaniesIDs.Contains(companyId))
									.ToList()
			};

			var entries = await _repoTrialBalance.GetAsync(queryTrialBalance);
			var groupedEntries = entries
				.GroupBy(e => new { e.AccountCode, e.AccountName, e.AccountType, e.CurrencyId })
				.Select(g => new
				{
					AccountCode = g.Key.AccountCode,
					AccountName = g.Key.AccountName,
					AccountType = g.Key.AccountType,
					CurrencyId = g.Key.CurrencyId,
					TotalDebit = g.Where(e => e.IsDebit).Sum(e => e.Amount),
					TotalCredit = g.Where(e => !e.IsDebit).Sum(e => e.Amount)
				})
				.OrderBy(item => item.AccountCode)
				.ToList();

			var reportItemsTasks = groupedEntries.Select(async item => new ReportTrialBalance
			{
				Name = $"{item.AccountCode} - {item.AccountName}",
				CurrencyName = await GetCurrencyName(item.CurrencyId),
				Balance = HelperReports.CalculateAccountBalance(item.AccountType, item.TotalDebit, item.TotalCredit)
			}).ToList();

			var reportItems = await Task.WhenAll(reportItemsTasks);

			return new ResponsePaged<ReportTrialBalance>
			{
				Items = reportItems.ToList(),
				TotalCount = reportItems.Count(),
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
	}
}
