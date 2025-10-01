using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IAuthorizationUserService
	{
		Task<ResponseJwtToken> LoginAsync(UserLoginDto dto);
	}
}
