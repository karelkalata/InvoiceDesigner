using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvoiceDesigner.API.Helpers
{
	public class ValidateUserIdFilter : IAsyncActionFilter
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ValidateUserIdFilter(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}


		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var userIdClaim = context.HttpContext.User.FindFirst("userId")?.Value;
			var isAdminClaim = context.HttpContext.User.FindFirst("isAdmin")?.Value;

			if (!int.TryParse(userIdClaim, out int userId))
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			context.HttpContext.Items["userId"] = userId;
			context.HttpContext.Items["isAdmin"] = bool.TryParse(isAdminClaim, out var isAdmin) && isAdmin;

			await next();
		}
	}
}
