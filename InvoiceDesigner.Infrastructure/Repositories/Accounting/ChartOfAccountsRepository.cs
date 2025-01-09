using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Accounting
{
	public class ChartOfAccountsRepository : IChartOfAccountsRepository
	{
		private readonly DataContext _context;

		public ChartOfAccountsRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<ChartOfAccounts>> GetEntitiesAsync(QueryPaged queryPaged, Func<IQueryable<ChartOfAccounts>, IOrderedQueryable<ChartOfAccounts>> orderBy)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<ChartOfAccounts> query = _context.ChartOfAccounts.AsNoTracking();

			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				var searchString = queryPaged.SearchString.ToLower();
				query = query.Where(c => c.Name.ToLower().Contains(searchString));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}

		public async Task<int> CreateAsync(ChartOfAccounts entity)
		{
			await _context.ChartOfAccounts.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<ChartOfAccounts?> GetByIdAsync(int id)
		{
			return await _context.ChartOfAccounts
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<ChartOfAccounts?> GetByCodeAsync(int code)
		{
			return await _context.ChartOfAccounts
				.FirstOrDefaultAsync(c => c.Code == code);
		}
		public async Task<int> UpdateAsync(ChartOfAccounts entity)
		{
			_context.ChartOfAccounts.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteAsync(ChartOfAccounts entity)
		{
			_context.ChartOfAccounts.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountAsync()
		{
			return await _context.ChartOfAccounts.CountAsync();
		}


	}
}
