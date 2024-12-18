﻿using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.User;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.LoginAsync(userLogin);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
