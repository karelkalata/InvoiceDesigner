using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IInvoiceService
	{
		Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int userId, bool isAdmin, int invoiceId);

		Task<ResponsePaged<InvoicesViewDto>> GetPagedInvoicesAsync( int userId, 
																	bool isAdmin,
																	int pageSize,
																	int page,
																	string searchString,
																	string sortLabel);


		Task<ResponseRedirect> CreateInvoiceAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto);

		Task<Invoice?> GetInvoiceByIdAsync(int userId, bool isAdmin, int id);

		Task<InvoiceEditDto> GetInvoiceDtoByIdAsync(int userId, bool isAdmin, int id);

		Task<ResponseRedirect> UpdateInvoiceAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto);

		Task<ResponseBoolean> DeleteInvoiceAsync(int userId, bool isAdmin, int id);

		Task<int> GetInvoiceCountAsync(int userId, bool isAdmin );

	}
}
