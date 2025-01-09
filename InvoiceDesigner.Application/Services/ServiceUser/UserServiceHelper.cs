using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Services.ServiceUser
{
	public class UserServiceHelper : IUserServiceHelper
	{
		private readonly IUserRepository _repoUser;

		public UserServiceHelper(IUserRepository repoUser)
		{
			_repoUser = repoUser;
		}

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _repoUser.GetUserByIdAsync(id);
		}
	}
}
