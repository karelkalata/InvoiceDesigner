using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.InvoiceDTOs;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

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
		Task<ResponseBoolean> OnChangeProperty(QueryOnChangeProperty queryStatus);
	}
}
