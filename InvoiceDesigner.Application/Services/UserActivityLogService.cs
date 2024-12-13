using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.DtoActivityLog;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Services
{
	public class UserActivityLogService : IUserActivityLogService
	{
		private readonly IUserActivityLogRepository _repoUserActivityLog;
		private readonly IMapper _mapper;
		private readonly IUserServiceHelper _userServiceHelper;

		public UserActivityLogService(IUserActivityLogRepository repoUserActivityLog,
										IMapper mapper,
										IUserServiceHelper userServiceHelper)
		{
			_repoUserActivityLog = repoUserActivityLog;
			_mapper = mapper;
			_userServiceHelper = userServiceHelper;
		}

		public async Task<ResponsePaged<UserActivityLogViewDto>> GetPagedAsync(QueryPagedActivityLogs queryPaged)
		{
			queryPaged.PageSize = Math.Max(queryPaged.PageSize, 1);
			queryPaged.Page = Math.Max(queryPaged.Page, 1);

			var activityLogsTask = _repoUserActivityLog.GetEntitiesAsync(queryPaged);
			var totalCountTask = _repoUserActivityLog.GetCountAsync(queryPaged);

			await Task.WhenAll(activityLogsTask, totalCountTask);

			var activityLogsViewDto = _mapper.Map<IReadOnlyCollection<UserActivityLogViewDto>>(await activityLogsTask);
			var result = new ResponsePaged<UserActivityLogViewDto>
			{
				Items = activityLogsViewDto,
				TotalCount = await totalCountTask
			};

			return result;
		}

		public async Task UserLogout(User user)
		{
			var log = new UserActivityLog
			{
				UserId = user.Id,
				User = user,
				DocumentTypes = EDocumentsTypes.User,
				ActivitiesType = EActivitiesType.Logout,
				EntityId = user.Id
			};

			await _repoUserActivityLog.CreateAsync(log);
		}

		public async Task UserLogin(User user, EActivitiesType activitiesType)
		{
			var log = new UserActivityLog
			{
				UserId = user.Id,
				User = user,
				DocumentTypes = EDocumentsTypes.User,
				ActivitiesType = activitiesType,
				EntityId = user.Id
			};

			await _repoUserActivityLog.CreateAsync(log);
		}

		public async Task CreateActivityLog(int userId, EDocumentsTypes documentType, EActivitiesType activitiesType, int? entityId = null, string? entityNumber = null)
		{
			var user = await _userServiceHelper.GetByIdAsync(userId)
				?? throw new UnauthorizedAccessException("Access denied.");

			var log = new UserActivityLog
			{
				UserId = user.Id,
				User = user,
				DocumentTypes = documentType,
				ActivitiesType = activitiesType,
				EntityId = entityId,
				EntityNumber = entityNumber
			};

			await _repoUserActivityLog.CreateAsync(log);

		}
	}
}
