using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
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
		public IActionResult Logout()
		{
			return NoContent();
		}
	}
}
