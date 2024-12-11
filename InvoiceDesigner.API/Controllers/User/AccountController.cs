using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.User
{
	[Route("api/User/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IUserService _service;

		public AccountController(IUserService service)
		{
			_service = service;
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserEditDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdAsync(int id = 0)
		{
			try
			{
				var (userId, isAdmin) = GetValidatedFilters();  // do not believe the data in the dto sent by the user
				var result = await _service.GetEditDtoByIdAsync(userId);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody] UserEditDto userEditDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var (userId, isAdmin) = GetValidatedFilters();
				var result = await _service.UpdateAsync(userId, userEditDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
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
