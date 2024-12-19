using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Abstract
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public abstract class RESTController : ControllerBase
	{
		protected int UserId => GetUserId();
		protected bool IsAdmin => GetIsAdmin();

		private int GetUserId()
		{
			var userIdString = User.FindFirst("userId")?.Value;
			return int.TryParse(userIdString, out int userId) ? userId : 0;
		}

		private bool GetIsAdmin()
		{
			var isAdminString = User.FindFirst("isAdmin")?.Value;
			return bool.TryParse(isAdminString, out bool isAdmin) && isAdmin;
		}
	}
}
