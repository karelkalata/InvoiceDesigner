using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Repositories
{
	public class UserActivityLogRepository : IUserActivityLogRepository
	{
		private readonly DataContext _context;

		public UserActivityLogRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<IReadOnlyCollection<UserActivityLog>> GetEntitiesAsync(QueryPagedActivityLogs queryPaged)
		{
			int skip = (queryPaged.Page - 1) * queryPaged.PageSize;

			IQueryable<UserActivityLog> query = _context.UserActivityLogs.AsNoTracking();

			if (queryPaged.DocumentTypes != null)
			{
				query = query.Where(e => e.DocumentTypes == queryPaged.DocumentTypes);
			}

			if (queryPaged.EntityId != null)
			{
				query = query.Where(e => e.EntityId == queryPaged.EntityId);
			}

			query = query.OrderByDescending(e => e.DateTime);

			var result = await query
				.Include(a => a.User)
				.Skip(skip)
				.Take(queryPaged.PageSize)
				.ToListAsync();

			return result;
		}

		public async Task<bool> CreateAsync(UserActivityLog entity)
		{
			await _context.UserActivityLogs.AddAsync(entity);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<int> GetCountAsync(QueryPagedActivityLogs queryPaged)
		{
			IQueryable<UserActivityLog> query = _context.UserActivityLogs.AsNoTracking();

			if (queryPaged.DocumentTypes != null)
			{
				query = query.Where(e => e.DocumentTypes == queryPaged.DocumentTypes);
			}

			if (queryPaged.EntityId != null)
			{
				query = query.Where(e => e.EntityId == queryPaged.EntityId);
			}

			return await query.CountAsync();
		}

	}
}
