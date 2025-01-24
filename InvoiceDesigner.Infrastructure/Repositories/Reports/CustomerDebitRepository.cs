using InvoiceDesigner.Domain.Shared.DTOs.Reports.CustomerDebit;
using InvoiceDesigner.Domain.Shared.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Reports
{
	public class CustomerDebitRepository : ICustomerDebitRepository
	{
		private readonly DataContext _context;

		public CustomerDebitRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<CustomerDebitDto>> GetAsync(QueryCustomerDebit query, ChartOfAccounts chartOfAccounts)
		{
			IQueryable<DoubleEntry> entries = _context.Accounting
				.AsNoTracking()
				.Include(e => e.DebitAccount)
				.Include(e => e.CreditAccount);


			if (query.CompaniesIDs != null && query.CompaniesIDs.Any())
			{
				entries = entries.Where(e => query.CompaniesIDs.Contains(e.CompanyId));
			}

			entries = entries.Where(e => e.DebitAccount == chartOfAccounts || e.CreditAccount == chartOfAccounts);

			if (query.CustomerId  > 0)
			{
				entries = entries.Where(e => (e.DebitAccount == chartOfAccounts && e.DebitAsset1 == query.CustomerId) ||
											 (e.CreditAccount == chartOfAccounts && e.CreditAsset1 == query.CustomerId));
			}

			return await entries
				.Select(e => new CustomerDebitDto
				{
					Amount = e.Amount,
					IsDebit = e.DebitAccount == chartOfAccounts,
					CurrencyId = e.CurrencyId,
					CustomerId = e.DebitAccount == chartOfAccounts ? e.DebitAsset1 : e.CreditAsset1
				}).ToListAsync();
		}
	}
}
