using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces.Admin
{
	public interface ICompanyService : IABaseService<Company>
	{
		Task<ResponsePaged<CompanyViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand);




		Task<ResponseRedirect> CreateAsync(int userId, CompanyEditDto companyCreateDto);
		Task<Company> GetByIdAsync(int id);
		Task<CompanyEditDto> GetEditDtoByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, CompanyEditDto companyCreateDto);
		Task<int> GetCountAsync();
		Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId, bool isAdmin);
		Task<IReadOnlyCollection<CompanyAutocompleteDto>> GetAllAutocompleteDto(int userId, bool isAdmin);
		Task<IReadOnlyCollection<CompanyAutocompleteDto>> FilteringData(string f);
	}
}
