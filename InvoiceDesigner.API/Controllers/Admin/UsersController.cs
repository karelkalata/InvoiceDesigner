using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.User;
using InvoiceDesigner.Application.Interfaces.AdminInterfaces;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Admin
{
	[Authorize(Policy = UserPolicy.IsAdmin)]
	[Route("api/Admin/[controller]")]
	public class UsersController : RESTController
	{
		private readonly IAdminUserInterface _serviceAdminUser;

		public UsersController(IAdminUserInterface serviceAdminUser)
		{
			_serviceAdminUser = serviceAdminUser;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<UserViewDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPaged queryPaged)
		{
			try
			{
				var result = await _serviceAdminUser.GetPagedEntitiesAsync(queryPaged);
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

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody] AdminUserEditDto adminUserEditDto)
		{
			try
			{
				var result = await _serviceAdminUser.CreateUserAsync(UserId, adminUserEditDto);
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

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminUserEditDto))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			try
			{
				var result = await _serviceAdminUser.GetEditDtoByIdAsync(id);
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
		public async Task<IActionResult> UpdateAsync([FromBody] AdminUserEditDto adminUserEditDto)
		{
			try
			{
				var result = await _serviceAdminUser.UpdateAsync(UserId, adminUserEditDto);
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

		[HttpDelete("{id:int}/{modeDelete:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteOrMarkAsDeletedAsync(int id, int modeDelete)
		{
			try
			{
				var deleteEntityCommand = new DeleteEntityCommand
				{
					UserId = UserId,
					IsAdmin = IsAdmin,
					EntityId = id,
					MarkAsDeleted = modeDelete == 0
				};

				var result = await _serviceAdminUser.DeleteOrMarkAsDeletedAsync(deleteEntityCommand);
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

		[HttpGet("CheckLoginName")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CheckLoginName(string f = "")
		{
			try
			{
				var result = await _serviceAdminUser.CheckLoginName(f);
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
