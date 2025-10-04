using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Documents
{
	public class InvoiceRepository : ABaseRepository<Invoice>, IInvoiceRepository
	{
		public InvoiceRepository(DataContext context) : base(context) { }

		public override async Task<IReadOnlyCollection<Invoice>> GetEntitiesAsync(PagedFilter pagedFilter)
		{
			int skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

			IQueryable<Invoice> query = _context.Invoices.AsNoTracking();


			if (!pagedFilter.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!pagedFilter.ShowArchived)
			{
				query = query.Where(e => e.IsArchived == false);
			}

			if (!string.IsNullOrEmpty(pagedFilter.SearchString))
			{
				var searchString = pagedFilter.SearchString.ToLower();
				query = query.Where(c => c.Company.Name.ToLower().Contains(pagedFilter.SearchString) || c.Customer.Name.ToLower().Contains(pagedFilter.SearchString));
			}

			if (!string.IsNullOrEmpty(pagedFilter.SortLabel))
			{
				query = GetOrdering(pagedFilter.SortLabel)(query);
			}

			if (pagedFilter.UserAuthorizedCompanies != null)
			{
				query = query.Where(invoice => pagedFilter.UserAuthorizedCompanies.Contains(invoice.Company));
			}

			return await query
				.Include(a => a.Company)
				.Include(b => b.Currency)
				.Include(c => c.Customer)
				.Skip(skip)
				.Take(pagedFilter.PageSize)
				.ToListAsync();
		}

		public override async Task<Invoice?> GetByIdAsync(GetByIdFilter getByIdFilter)
		{
			return await _context.Invoices
				.Where(invoice => getByIdFilter.userAuthorizedCompanies.Contains(invoice.Company))
				.Include(a => a.Currency)
				.Include(b => b.Bank)
				.Include(c => c.Customer)
				.Include(d => d.InvoiceItems)
					.ThenInclude(ii => ii.Item)
				.Include(e => e.Company)
					.ThenInclude(f => f.Currency)
				.Where(c => c.Id == getByIdFilter.Id)
				.SingleOrDefaultAsync();
		}


		public async Task<int> GetNextNumberForCompanyAsync(int companyId)
		{
			var lastInvoiceNumber = await _context.Invoices
				.Where(i => i.CompanyId == companyId)
				.OrderByDescending(i => i.Number)
				.Select(i => i.Number)
				.FirstOrDefaultAsync();

			return lastInvoiceNumber + 1;
		}

		public async Task<bool> IsCompanyUsed(int companyId)
		{
			return await _context.Invoices.AnyAsync(a => a.CompanyId == companyId);
		}

		public async Task<bool> IsBankUsed(int bankId)
		{
			return await _context.Invoices.AnyAsync(a => a.BankId == bankId);
		}

		public async Task<bool> IsClientUsed(int clientId)
		{
			return await _context.Invoices.AnyAsync(a => a.CustomerId == clientId);
		}

		public async Task<bool> IsCurrencyUsed(int currencyId)
		{
			return await _context.Invoices.AnyAsync(a => a.CurrencyId == currencyId);
		}

		public async Task<bool> IsProductUsed(int productId)
		{
			return await _context.InvoiceItems.AnyAsync(x => x.ItemId == productId);
		}

		public Task<IReadOnlyCollection<Invoice>> GetAsync(PagedFilter pagedFilter, Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetCountAsync(PagedFilter pagedFilter)
		{
			throw new NotImplementedException();
		}
	}
}
