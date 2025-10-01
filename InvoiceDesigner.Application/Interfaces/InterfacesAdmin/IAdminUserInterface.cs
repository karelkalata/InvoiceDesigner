using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Application.Interfaces.AdminInterfaces
{
	public interface IAdminUserInterface
	{
		Task<ResponsePaged<UserViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateUserAsync(int userId, AdminUserEditDto dto);

		Task<AdminUserEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, AdminUserEditDto dto);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand);

		Task<ResponseBoolean> CheckLoginName(string loginName);
	}
}
