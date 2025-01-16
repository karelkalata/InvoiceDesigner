using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Documents
{
	public class BankReceiptRepository : IBankReceiptRepository
	{
		private readonly DataContext _context;

		public BankReceiptRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<BankReceipt>> GetAsync(QueryPaged queryPaged, Func<IQueryable<BankReceipt>, IOrderedQueryable<BankReceipt>> orderBy, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<BankReceipt> query = _context.BankReceipts.AsNoTracking()
												.AsNoTracking()
												.Where(invoice => userAuthorizedCompanies.Contains(invoice.Company))
												.Include(a => a.Company)
												.Include(a => a.Bank)
												.Include(a => a.Currency)
												.Include(a => a.Customer);

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

		public async Task<int> CreateAsync(BankReceipt entity)
		{
			await _context.BankReceipts.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<BankReceipt?> GetByIdAsync(int id, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			return await _context.BankReceipts
				.Where(e => userAuthorizedCompanies.Contains(e.Company))
				.Include(a => a.Currency)
				.Include(b => b.Bank)
				.Include(c => c.Customer)
				.Include(e => e.Invoice)
				.Include(e => e.Company)
					.ThenInclude(f => f.Currency)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<BankReceipt?> GetByInvoiceIdAsync(int invoiceId, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			return await _context.BankReceipts
				.Where(e => userAuthorizedCompanies.Contains(e.Company))
				.Include(a => a.Currency)
				.Include(b => b.Bank)
				.Include(c => c.Customer)
				.Include(e => e.Company)
					.ThenInclude(f => f.Currency)
				.Where(c => c.InvoiceId == invoiceId)
				.SingleOrDefaultAsync();
		}

		public async Task<int> UpdateAsync(BankReceipt entity)
		{
			_context.BankReceipts.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteAsync(BankReceipt entity)
		{
			_context.BankReceipts.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountAsync(QueryPaged queryPaged, IReadOnlyCollection<Company> userAuthorizedCompanies)
		{
			IQueryable<BankReceipt> query = _context.BankReceipts.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			if (!queryPaged.ShowArchived)
			{
				query = query.Where(e => e.IsArchived == false);
			}

			return await query
						.Where(e => userAuthorizedCompanies.Contains(e.Company))
						.CountAsync();
		}

		public async Task<int> GetNextNumberForCompanyAsync(int companyId)
		{
			var lastNumber = await _context.BankReceipts
				.Where(i => i.CompanyId == companyId)
				.OrderByDescending(i => i.Number)
				.Select(i => i.Number)
				.FirstOrDefaultAsync();

			return lastNumber + 1;
		}

	}
}
