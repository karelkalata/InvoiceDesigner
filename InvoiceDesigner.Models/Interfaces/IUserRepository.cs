using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IUserRepository
	{
		Task<IReadOnlyCollection<User>> GetUsersAsync(int pageSize,
														int pageNumber,
														string searchString,
														Func<IQueryable<User>, IOrderedQueryable<User>> orderBy);

		Task<int> CreateUserAsync(User entity);

		Task<User?> GetUserByIdAsync(int id);

		Task<User?> GetUserByLoginAsync(string loginName);

		Task<int> UpdateUserAsync(User entity);

		Task<bool> DeleteUserAsync(User entity);

		Task<int> GetCountUsersAsync();
	}
}
