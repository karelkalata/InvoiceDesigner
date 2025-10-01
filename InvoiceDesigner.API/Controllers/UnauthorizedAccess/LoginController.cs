﻿using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Unauthorized
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly IAuthorizationUserService _service;

		public LoginController(IAuthorizationUserService service)
		{
			_service = service;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseJwtToken))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLogin)
		{
			try
			{
				var result = await _service.LoginAsync(userLogin);
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
