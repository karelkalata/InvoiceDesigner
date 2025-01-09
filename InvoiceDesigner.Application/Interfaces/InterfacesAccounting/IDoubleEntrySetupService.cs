using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesAccounting
{
	public interface IDoubleEntrySetupService
	{
		Task<ResponsePaged<DoubleEntrySetupEditDto>> GetPagedAsync(QueryPagedDoubleEntrySetup queryPaged);
		Task<List<DoubleEntrySetup>> GetEntitiesAsync(QueryPagedDoubleEntrySetup queryPaged);
		Task<ResponseRedirect> CreateAsync(DoubleEntrySetupEditDto createDto);
		Task<ResponseRedirect> UpdateAsync(DoubleEntrySetupEditDto editedDto);
		Task<ResponseBoolean> DeleteAsync(int id);
		Task<int> GetCountByTypeDocumentAsync(EAccountingDocument typeDocument);
	}
}
