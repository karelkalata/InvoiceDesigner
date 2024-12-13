using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.AdminInterfaces
{
	public interface IAdminUserInterface
	{
		Task<ResponsePaged<UserViewDto>> GetPagedEntitiesAsync(QueryPaged queryPaged);

		Task<ResponseRedirect> CreateUserAsync(int userId, AdminUserEditDto dto);

		Task<AdminUserEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, AdminUserEditDto dto);

		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(QueryDeleteEntity queryDeleteEntity);

		Task<ResponseBoolean> CheckLoginName(string loginName);
	}
}
