using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.DtoActivityLog;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserActivityLogController : Controller
	{
		private readonly IUserActivityLogService _serviceUserActivityLog;

		public UserActivityLogController(IUserActivityLogService serviceUserActivityLog)
		{
			_serviceUserActivityLog = serviceUserActivityLog;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<UserActivityLogViewDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPagedActivityLogs queryPagedActivityLogs)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _serviceUserActivityLog.GetPagedAsync(queryPagedActivityLogs);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			}

		}
	}
}
