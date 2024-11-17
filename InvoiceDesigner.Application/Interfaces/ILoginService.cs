using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface ILoginService
	{
		Task<ResponseJwtToken> LoginAsync(UserLoginDto dto);
	}
}
