using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICompanyService
	{
		Task<ResponsePaged<CompanyViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateAsync(CompanyEditDto companyCreateDto);

		Task<Company> GetByIdAsync(int id);

		Task<CompanyEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(CompanyEditDto companyCreateDto);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity);

		Task<int> GetCountAsync();

		Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin);

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllAutocompleteDto(int userId, bool isAdmin);

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string f);
	}
}
