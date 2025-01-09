using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesAccounting
{
	public interface IChartOfAccountsService
	{
		Task<ResponsePaged<ChartOfAccountsDto>> GetPagedAsync(QueryPaged queryPaged);
		Task<ResponseRedirect> CreateAsync(ChartOfAccountsDto createDto);
		Task<ChartOfAccounts> ValidateExistsEntityAsync(int id);
		Task<ResponseRedirect> UpdateAsync(ChartOfAccountsDto editedDto);
		Task<ResponseBoolean> DeleteAsync(int id);
		Task<IReadOnlyCollection<ChartOfAccountsAutocompleteDto>> FilteringData(string f);
	}
}
