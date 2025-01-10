using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Directories
{
	public interface IUserRepository : IABaseRepository<User>
	{
		Task<User?> GetByLoginAsync(string loginName);
	}
}
