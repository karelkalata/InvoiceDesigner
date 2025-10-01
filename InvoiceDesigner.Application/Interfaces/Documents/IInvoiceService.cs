using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs;
using InvoiceDesigner.Application.DTOs.InvoiceDTOs;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Interfaces.Documents
{
	public interface IInvoiceService
	{
		Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int userId, bool isAdmin, int invoiceId);
		Task<ResponsePaged<InvoicesViewDto>> GetPagedAsync(QueryPaged queryPaged);
		Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto);
		Task<Invoice?> GetByIdAsync(int userId, bool isAdmin, int id);
		Task<InvoiceEditDto> GetDtoByIdAsync(int userId, bool isAdmin, int id);
		Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto);
		Task<ResponseBoolean> DeleteAsync(int userId, bool isAdmin, int id);
		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(int userId, bool isAdmin, int id, int modeDelete);
		Task<ResponseBoolean> OnChangeProperty(ChangePropertyCommand changePropertyCommand);
	}
}
