using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Authorization
{
	[Route("api/[controller]/[action]")]
	public class CultureController : ControllerBase
	{
		public IActionResult Set(string culture, string redirectUri)
		{
			HttpContext.Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(
					new RequestCulture(culture, culture)));

			return Redirect(redirectUri);
		}
	}
}
