using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICompanyService
	{
		Task<PagedResult<CompanyViewDto>> GetPagedCompaniesAsync(int pageSize,
																	int page,
																	string searchString,
																	string sortLabel);

		Task<ResponseRedirect> CreateCompanyAsync(CompanyEditDto companyCreateDto);

		Task<Company> GetCompanyByIdAsync(int id);

		Task<CompanyEditDto> GetCompanyEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateCompanyAsync(CompanyEditDto companyCreateDto);

		Task<bool> DeleteCompanyAsync(int id);

		Task<int> GetCompaniesCountAsync();

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllCompanyAutocompleteDto();

		Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string f);
	}
}
