using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.User
{
	[Route("api/User/[controller]")]
	public class AccountController : RESTController
	{
		private readonly IUserService _service;

		public AccountController(IUserService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserEditDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Account()
		{
			try
			{
				var result = await _service.GetEditDtoByIdAsync(UserId);
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

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody] UserEditDto userEditDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var result = await _service.UpdateAsync(UserId, userEditDto);
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
