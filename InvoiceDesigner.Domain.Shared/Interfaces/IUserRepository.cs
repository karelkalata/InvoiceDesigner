using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IUserRepository
	{
		Task<IReadOnlyCollection<User>> GetUsersAsync(QueryPaged queryPaged, Func<IQueryable<User>, IOrderedQueryable<User>> orderBy);

		Task<int> CreateUserAsync(User entity);

		Task<User?> GetUserByIdAsync(int id);

		Task<User?> GetUserByLoginAsync(string loginName);

		Task<int> UpdateUserAsync(User entity);

		Task<bool> DeleteUserAsync(User entity);

		Task<int> GetCountUsersAsync(bool showDeleted = false);
	}
}
