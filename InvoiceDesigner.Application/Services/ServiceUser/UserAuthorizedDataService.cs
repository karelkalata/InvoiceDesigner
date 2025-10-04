using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.Filters;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Services.ServiceUser
{
	public class UserAuthorizedDataService : IUserAuthorizedDataService
	{
		private readonly IUserRepository _repository;

		public UserAuthorizedDataService(IUserRepository repository)
		{
			_repository = repository;
		}

		public async Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId)
		{
			var user = await _repository.GetByIdAsync(new GetByIdFilter { Id = userId });
			if (user == null)
				return new List<Company>();

			return user.Companies.ToList();

		}
	}
}
