using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IUserService
	{

		Task<UserEditDto> GetEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateAsync(int userId, UserEditDto dto);

	}
}
