using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IUserServiceHelper
	{
		Task<User?> GetByIdAsync(int id);
	}
}
