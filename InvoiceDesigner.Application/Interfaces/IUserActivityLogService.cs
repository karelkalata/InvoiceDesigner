using InvoiceDesigner.Domain.Shared.DTOs.DtoActivityLog;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IUserActivityLogService
	{
		Task<ResponsePaged<UserActivityLogViewDto>> GetPagedAsync(QueryPagedActivityLogs queryPagedActivityLogs);

		Task UserLogout(User user);
		Task UserLogin(User user, EActivitiesType status);

		Task CreateActivityLog(int userId, EDocumentsTypes documentType, EActivitiesType activitiesType, int? entityId = null, string? entityNumber = null);
	}
}
