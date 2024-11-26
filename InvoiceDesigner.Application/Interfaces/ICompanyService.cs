using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICompanyService
	{
		Task<ResponsePaged<CompanyViewDto>> GetPagedCompaniesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateCompanyAsync(CompanyEditDto companyCreateDto);

		Task<Company> GetCompanyByIdAsync(int id);

		Task<CompanyEditDto> GetCompanyEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateCompanyAsync(CompanyEditDto companyCreateDto);

		Task<ResponseBoolean> DeleteCompanyAsync(int id);

		Task<int> GetCompaniesCountAsync();

		Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin);

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllCompanyAutocompleteDto(int userId, bool isAdmin);

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string f);
	}
}
