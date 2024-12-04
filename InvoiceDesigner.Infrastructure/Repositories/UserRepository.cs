using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			_context = context;
		}


		public async Task<IReadOnlyCollection<User>> GetUsersAsync(QueryPaged queryPaged, Func<IQueryable<User>, IOrderedQueryable<User>> orderBy)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<User> query = _context.Users.AsNoTracking();

			if (!queryPaged.ShowDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);

			}
			if (!string.IsNullOrEmpty(queryPaged.SearchString))
			{
				var searchString = queryPaged.SearchString.ToLower();
				query = query.Where(c => c.Name.ToLower().Contains(searchString) || c.Login.ToLower().Contains(searchString));
			}

			query = orderBy(query);

			return await query
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();
		}

		public async Task<int> CreateUserAsync(User entity)
		{
			await _context.Users.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<User?> GetUserByIdAsync(int id)
		{
			return await _context.Users
				.Where(c => c.Id == id)
				.Include(c => c.Companies)
					.ThenInclude(company => company.Banks) 
						.ThenInclude(bank => bank.Currency)
				.SingleOrDefaultAsync();
		}

		public async Task<User?> GetUserByLoginAsync(string loginName)
		{
			return await _context.Users
				.Where(c => c.Login == loginName)
				.SingleOrDefaultAsync();
		}

		public async Task<int> UpdateUserAsync(User entity)
		{
			_context.Users.Update(entity);
			await _context.SaveChangesAsync();
			return entity.Id;
		}

		public async Task<bool> DeleteUserAsync(User entity)
		{
			_context.Users.Remove(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountUsersAsync(bool showDeleted = false)
		{
			IQueryable<User> query = _context.Users;

			if (!showDeleted)
			{
				query = query.Where(e => e.IsDeleted == false);
			}

			return await query.CountAsync();
		}

	}
}
