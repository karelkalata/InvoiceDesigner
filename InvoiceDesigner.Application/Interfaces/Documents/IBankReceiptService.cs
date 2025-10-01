using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.BankReceiptDTOs;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Interfaces.Documents
{
	public interface IBankReceiptService
	{
		Task<ResponsePaged<BankReceiptViewDto>> GetPagedAsync(QueryPaged queryPaged);
		Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, BankReceiptCreateDto editedEntity);
		Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, BankReceiptCreateDto editedEntity);
		Task<BankReceiptViewDto> GetDtoByIdAsync(GetEntityCommand getEntityCommand);
		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand);
		Task<ResponseBoolean> OnChangeProperty(ChangePropertyCommand changePropertyCommand);
	}
}
