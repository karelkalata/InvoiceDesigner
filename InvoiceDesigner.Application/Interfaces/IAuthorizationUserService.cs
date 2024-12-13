using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IAuthorizationUserService
	{
		Task<ResponseJwtToken> LoginAsync(UserLoginDto dto);
		Task LogoutUser(int userId);
	}
}
