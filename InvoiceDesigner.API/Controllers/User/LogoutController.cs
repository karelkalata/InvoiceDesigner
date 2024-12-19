using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.User
{
	public class LogoutController : RESTController
	{
		private readonly IAuthorizationUserService _service;

		public LogoutController(IAuthorizationUserService service) : base()
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
				await _service.LogoutUser(UserId);
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
	}
}
