using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs;
using InvoiceDesigner.Application.DTOs.Invoice;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Interfaces.Documents
{
	public interface IInvoiceService : IABaseService<Invoice>
	{
		Task<ResponsePaged<InvoicesViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand);


		Task<InfoForNewInvoiceDto> GetInfoForNewInvoice(int userId, bool isAdmin, int invoiceId);
		Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto);
		Task<Invoice?> GetByIdAsync(int userId, bool isAdmin, int id);
		Task<InvoiceEditDto> GetDtoByIdAsync(int userId, bool isAdmin, int id);
		Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, InvoiceEditDto invoiceDto);
		Task<ResponseBoolean> DeleteAsync(int userId, bool isAdmin, int id);

		Task<ResponseBoolean> OnChangeProperty(ChangePropertyCommand changePropertyCommand);
	}
}
