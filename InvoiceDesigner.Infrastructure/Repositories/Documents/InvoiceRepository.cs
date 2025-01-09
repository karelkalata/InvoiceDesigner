using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Documents
{
	public class InvoiceRepository : IInvoiceRepository
	{
		private readonly DataContext _context;

		public InvoiceRepository(DataContext context)
		{
			_context = context;
		}


		public async Task<IReadOnlyCollection<Invoice>> GetAsync(QueryPaged queryPaged,
																			Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy,
																			IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<Invoice> query = _context.Invoices.AsNoTracking()
												.AsNoTracking()
												.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
												.Include(a => a.Company)
												.Include(b => b.Currency)
												.Include(c => c.Customer);

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!queryPaged.ShowArchived)
			{
				query = query.Where(e => e.IsArchived == false);
			}

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				queryPaged.SearchString = queryPaged.SearchString.ToLower();
				query = query.Where(c => c.Company.Name.ToLower().Contains(queryPaged.SearchString) || c.Customer.Name.ToLower().Contains(queryPaged.SearchString));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}

		public async Task<int> CreateAsync(Invoice entity)
		{
			await _context.Invoices.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<Invoice?> GetByIdAsync(int id, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			return await _context.Invoices
				.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
				.Include(a => a.Currency)
				.Include(b => b.Bank)
				.Include(c => c.Customer)
				.Include(d => d.InvoiceItems)
					.ThenInclude(ii => ii.Item)
				.Include(e => e.Company)
					.ThenInclude(f => f.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<int> UpdateAsync(Invoice entity)
		{
			_context.Invoices.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteAsync(Invoice entity)
		{
			_context.Invoices
				.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountAsync(QueryPaged queryPaged, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			IQueryable<Invoice> query = _context.Invoices.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!queryPaged.ShowArchived)
			{
				query = query.Where(e => e.IsArchived == false);
			}

			return await query
						.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
						.CountAsync();
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

		public async Task<bool> IsProductUsedInInvoiceItems(int productId)
		{
			return await _context.InvoiceItems.AnyAsync(x => x.ItemId == productId);
		}
	}
}
