using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IPrintInvoiceService
	{
		Task<PdfFileInfo> CreatePDF(int invoiceId, int printform);
	}
}
