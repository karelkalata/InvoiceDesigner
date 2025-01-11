using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Accounting
{
	public class DoubleEntrySetupRepository : ABaseRepository<DoubleEntrySetup>, IDoubleEntrySetupRepository
	{
		public DoubleEntrySetupRepository(DataContext context) : base(context) { }

		public async Task<IReadOnlyCollection<DoubleEntrySetup>> GetEntitiesAsync(QueryPagedDoubleEntrySetup queryPaged)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<DoubleEntrySetup> query = _dbSet;

			return await query
				.Where(e => e.AccountingDocument == queryPaged.AccountingDocument)
				.Include(e => e.DebitAccount)
				.Include(e => e.CreditAccount)
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}


		public async Task<int> GetCountByTypeDocumentAsync(EAccountingDocument AccountingDocument)
		{
			return await _dbSet
				.Where(e => e.AccountingDocument == AccountingDocument)
				.CountAsync();
		}
	}
}
