using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Interfaces.Abstract;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces.AdminInterfaces
{
	public interface IAdminUserService : IABaseService<User>
	{
		Task<ResponsePaged<UserViewDto>> GetPagedEntitiesAsync(PagedCommand pagedCommand);


		Task<ResponseRedirect> CreateUserAsync(int userId, AdminUserEditDto dto);
		Task<AdminUserEditDto> GetEditDtoByIdAsync(int id);
		Task<ResponseRedirect> UpdateAsync(int userId, AdminUserEditDto dto);

		Task<ResponseBoolean> CheckLoginName(string loginName);
	}
}
