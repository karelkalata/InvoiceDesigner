using InvoiceDesigner.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Authorization
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class LogoutController : Controller
	{
		private readonly IAuthorizationUserService _service;

		public LogoutController(IAuthorizationUserService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Logout()
		{
			try
			{
				var (userId, isAdmin) = GetValidatedFilters();
				await _service.LogoutUser(userId);
				return NoContent();
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}

		private (int, bool) GetValidatedFilters()
		{
			var userIdString = User.FindFirst("userId")?.Value;
			int.TryParse(userIdString, out int userId);

			var isAdminString = User.FindFirst("isAdmin")?.Value;
			bool.TryParse(isAdminString, out bool isAdmin);

			return (userId, isAdmin);
		}
	}
}
