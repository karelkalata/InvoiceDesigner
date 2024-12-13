using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IUserActivityLogRepository
	{
		Task<IReadOnlyCollection<UserActivityLog>> GetEntitiesAsync(QueryPagedActivityLogs queryPaged);

		Task<bool> CreateAsync(UserActivityLog entity);

		Task<int> GetCountAsync(QueryPagedActivityLogs queryPaged);
	}
}
