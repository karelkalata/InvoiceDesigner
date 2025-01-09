using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IAuthorizationUserService
	{
		Task<ResponseJwtToken> LoginAsync(UserLoginDto dto);
	}
}
