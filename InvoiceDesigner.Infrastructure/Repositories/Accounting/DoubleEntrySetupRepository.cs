using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Accounting
{
	public class DoubleEntrySetupRepository : IDoubleEntrySetupRepository
	{
		private readonly DataContext _context;

		public DoubleEntrySetupRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<DoubleEntrySetup>> GetEntitiesAsync(QueryPagedDoubleEntrySetup queryPaged)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<DoubleEntrySetup> query = _context.DoubleEntriesSetup;

			return await query
				.Where(e => e.AccountingDocument == queryPaged.TypeDocument)
				.Include(e => e.DebitAccount)
				.Include(e => e.CreditAccount)
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}


		public async Task<int> CreateAsync(DoubleEntrySetup entity)
		{
			await _context.DoubleEntriesSetup.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<DoubleEntrySetup?> GetByIdAsync(int id)
		{
			return await _context.DoubleEntriesSetup
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<int> UpdateAsync(DoubleEntrySetup entity)
		{
			_context.DoubleEntriesSetup.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteAsync(DoubleEntrySetup entity)
		{
			_context.DoubleEntriesSetup.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountAsync()
		{
			return await _context.Accounting.CountAsync();
		}

		public async Task<int> GetCountByTypeDocumentAsync(EAccountingDocument typeDocument)
		{
			return await _context.DoubleEntriesSetup
				.Where(e => e.AccountingDocument == typeDocument)
				.CountAsync();
		}
	}
}
