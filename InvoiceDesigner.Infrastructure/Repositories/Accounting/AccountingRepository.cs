using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories.Accounting
{
	public class AccountingRepository : IAccountingRepository
	{
		private readonly DataContext _context;

		public AccountingRepository(DataContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(DoubleEntry entity)
		{
			await _context.Accounting.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(EAccountingDocument typeDocument, int documentId)
		{
			var entriesToDelete = await _context.Accounting
				.Where(e => e.AccountingDocument == typeDocument && e.DocumentId == documentId)
				.ToListAsync();

			if (entriesToDelete.Any())
			{
				_context.Accounting.RemoveRange(entriesToDelete);
				await _context.SaveChangesAsync();
			}
		}
	}
}
