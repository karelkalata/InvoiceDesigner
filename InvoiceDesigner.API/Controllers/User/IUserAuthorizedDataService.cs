using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.API.Controllers.User
{
	public interface IUserAuthorizedDataService
	{
		Task<List<Company>> GetAuthorizedCompaniesAsync(int userId);
	}
}
