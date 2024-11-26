using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IUserService
	{
		Task<ResponsePaged<UserViewDto>> GetPagedUsersAsync(int pageSize, int page, string searchString, string sortLabel);

		Task<ResponseRedirect> CreateAdminUserAsync(AdminUserEditDto dto);

		Task<UserEditDto> GetUserEditDtoByIdAsync(int id);

		Task<AdminUserEditDto> GetAdminUserEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateUserAsync(UserEditDto dto);

		Task<ResponseRedirect> UpdateAdminUserAsync(AdminUserEditDto dto);

		Task<ResponseBoolean> DeleteUserAsync(int id);

		Task<ResponseBoolean> CheckLoginName(string loginName);

	}
}
