using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IUserServiceHelper
	{
		Task<User?> GetByIdAsync(int id);
	}
}
