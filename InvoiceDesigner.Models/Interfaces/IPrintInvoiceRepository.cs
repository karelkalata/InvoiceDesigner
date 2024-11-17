using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IPrintInvoiceRepository
	{
		Task<Guid> GenerateDownloadLinkAsync(PrintInvoice printInvoice);

		Task<PrintInvoice?> GetPrintInvoicebyGuidAsync(Guid guid);

		Task DeletePrintInvoicebyGuidAsync(PrintInvoice entity);
	}
}
