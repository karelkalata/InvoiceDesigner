using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class InvoiceRepository : IInvoiceRepository
	{
		private readonly DataContext _context;

		public InvoiceRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<Invoice>> GetInvoicesAsync(	int pageSize, 
																			int page, 
																			string searchString, 
																			Func<IQueryable<Invoice>, IOrderedQueryable<Invoice>> orderBy,
																			IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			int skip = (page - 1) * pageSize;

			IQueryable<Invoice> query = _context.Invoices.AsNoTracking()
												.AsNoTracking()
												.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
												.Include(a => a.Company)
												.Include(b => b.Currency)
												.Include(c => c.Customer);

			if (!string.IsNullOrEmpty(searchString))
			{
				searchString = searchString.ToLower();
				query = query.Where(c => c.Company.Name.ToLower().Contains(searchString) || c.Customer.Name.ToLower().Contains(searchString));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<int> CreateInvoiceAsync(Invoice entity)
		{
			await _context.Invoices.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<Invoice?> GetInvoiceByIdAsync(int id, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			return await _context.Invoices
				.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
				.Include(a => a.Currency)
				.Include(b => b.Bank)
				.Include(c => c.Customer)
				.Include(d => d.InvoiceItems)
					.ThenInclude(ii => ii.Product)
				.Include(e => e.Company)
					.ThenInclude(f => f.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<int> UpdateInvoiceAsync(Invoice entity)
		{
			_context.Invoices.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteInvoiceAsync(Invoice entity)
		{
			_context.Invoices
				.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountInvoicesAsync(IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			return await _context.Invoices
						.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
						.CountAsync();
		}

		public async Task<int> GetNextInvoiceNumberForCompanyAsync(int companyId)
		{
			var lastInvoiceNumber = await _context.Invoices
				.Where(i => i.CompanyId == companyId)
				.OrderByDescending(i => i.InvoiceNumber)
				.Select(i => i.InvoiceNumber)
				.FirstOrDefaultAsync();

			return lastInvoiceNumber + 1;
		}

		public async Task<bool> IsCompanyUsedInInvoices(int companyId)
		{
			return await _context.Invoices.AnyAsync(a => a.CompanyId == companyId);
		}

		public async Task<bool> IsBankUsedInInvoices(int bankId)
		{
			return await _context.Invoices.AnyAsync(a => a.BankId == bankId);
		}

		public async Task<bool> IsClientUsedInInvoices(int clientId)
		{
			return await _context.Invoices.AnyAsync(a => a.CustomerId == clientId);
		}

		public async Task<bool> IsCurrencyUsedInInvoices(int currencyId)
		{
			return await _context.Invoices.AnyAsync(a => a.CurrencyId == currencyId);
		}

		public async Task<bool> IsProductUsedInInvoiceItems(int productId)
		{
			return await _context.InvoiceItems.AnyAsync(x => x.ProductId == productId);
		}
	}
}
