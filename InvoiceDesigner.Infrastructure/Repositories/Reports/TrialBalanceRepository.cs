using InvoiceDesigner.Domain.Shared.DTOs.Reports;
using InvoiceDesigner.Domain.Shared.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Reports
{
	public class TrialBalanceRepository : ITrialBalanceRepository
	{
		private readonly DataContext _context;

		public TrialBalanceRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<TrialBalanceDto>> GetAsync(QueryTrialBalance query)
		{
			IQueryable<DoubleEntry> entries = _context.Accounting.AsNoTracking()
				.Include(e => e.DebitAccount)
				.Include(e => e.CreditAccount);

			if (query.DateStart.HasValue)
			{
				entries = entries.Where(e => e.DateTime >= query.DateStart.Value);
			}
			if (query.DateEnd.HasValue)
			{
				entries = entries.Where(e => e.DateTime <= query.DateEnd.Value);
			}

			if (query.CompaniesIDs != null && query.CompaniesIDs.Any())
			{
				entries = entries.Where(e => query.CompaniesIDs.Contains(e.CompanyId));
			}

			var debitEntries = entries.Select(e => new TrialBalanceDto
			{
				AccountCode = e.DebitAccount.Code,
				AccountName = e.DebitAccount.Name,
				AccountType = e.DebitAccount.TypeChartOfAccount,
				Amount = e.Amount,
				IsDebit = true,
				CurrencyId = e.CurrencyId
			});

			var creditEntries = entries.Select(e => new TrialBalanceDto
			{
				AccountCode = e.CreditAccount.Code,
				AccountName = e.CreditAccount.Name,
				AccountType = e.CreditAccount.TypeChartOfAccount,
				Amount = e.Amount,
				IsDebit = false,
				CurrencyId = e.CurrencyId
			});

			return await debitEntries.Concat(creditEntries).ToListAsync();
		}
	}
}
