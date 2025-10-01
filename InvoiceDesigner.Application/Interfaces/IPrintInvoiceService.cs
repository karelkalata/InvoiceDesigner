using InvoiceDesigner.Application.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IPrintInvoiceService
	{
		Task<ResponsePdf> CreatePDF(Guid guid);

		Task<ResponsePdfGuid> GenerateDownloadLink(int invoiceId, int printform);

	}
}
