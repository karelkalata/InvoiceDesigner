using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IInvoiceService
	{
		Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int invoiceId);

		Task<PagedResult<InvoicesViewDto>> GetPagedInvoicesAsync(int pageSize,
																int page,
																string searchString,
																string sortLabel);


		Task<ResponseRedirect> CreateInvoiceAsync(InvoiceEditDto invoiceDto);

		Task<Invoice?> GetInvoiceByIdAsync(int id);

		Task<InvoiceEditDto> GetInvoiceDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateInvoiceAsync(InvoiceEditDto invoiceDto);

		Task<bool> DeleteInvoiceAsync(int id);

		Task<int> GetInvoiceCountAsync();

	}
}
