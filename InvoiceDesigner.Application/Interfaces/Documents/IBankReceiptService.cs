using InvoiceDesigner.Domain.Shared.DTOs.BankReceiptDTOs;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.Documents
{
	public interface IBankReceiptService
	{
		Task<ResponsePaged<BankReceiptViewDto>> GetPagedAsync(QueryPaged queryPaged);
		Task<ResponseRedirect> CreateAsync(int userId, bool isAdmin, BankReceiptCreateDto editedEntity);
		Task<ResponseRedirect> UpdateAsync(int userId, bool isAdmin, BankReceiptCreateDto editedEntity);
		Task<BankReceiptViewDto> GetDtoByIdAsync(QueryGetEntity queryGetEntity);
		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity);
		Task<ResponseBoolean> OnChangeProperty(QueryOnChangeProperty queryOnChangeProperty);
	}
}
