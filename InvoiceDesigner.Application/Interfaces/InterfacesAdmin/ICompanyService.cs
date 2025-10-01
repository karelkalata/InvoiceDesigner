using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Interfaces.Admin
{
	public interface ICompanyService
	{
		Task<ResponsePaged<CompanyViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateAsync(int userId, CompanyEditDto companyCreateDto);

		Task<Company> GetByIdAsync(int id);

		Task<CompanyEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, CompanyEditDto companyCreateDto);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand);

		Task<int> GetCountAsync();

		Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin);

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllAutocompleteDto(int userId, bool isAdmin);

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string f);
	}
}
