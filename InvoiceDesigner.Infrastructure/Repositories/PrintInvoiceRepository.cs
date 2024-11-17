using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class PrintInvoiceRepository : IPrintInvoiceRepository
	{
		private readonly DataContext _context;

		public PrintInvoiceRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<Guid> GenerateDownloadLinkAsync(PrintInvoice entity)
		{
			await _context.PrintInvoices.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Giud;
		}

		public async Task<PrintInvoice?> GetPrintInvoicebyGuidAsync(Guid guid)
		{
			return await _context.PrintInvoices
				.FirstOrDefaultAsync(c => c.Giud == guid);
		}

		public async Task DeletePrintInvoicebyGuidAsync(PrintInvoice entity)
		{
			_context.PrintInvoices.Remove(entity);

			var outdatedRecords = _context.PrintInvoices
									.Where(e => e.CreatedAt < DateTime.Now.AddMinutes(-10))
									.ToList();

			_context.PrintInvoices.RemoveRange(outdatedRecords);
			await _context.SaveChangesAsync();
		}

	}
}
