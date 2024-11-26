using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Interfaces.InterfacesUser
{
	public interface IUserAuthorizedDataService
	{

		Task<IReadOnlyCollection<Company>> GetAuthorizedCompaniesAsync(int userId);

	}
}
