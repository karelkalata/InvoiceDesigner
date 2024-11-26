using InvoiceDesigner.API.Helpers;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.Invoice;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	[Authorize]
	[ServiceFilter(typeof(ValidateUserIdFilter))]
	[Route("api/[controller]")]
	[ApiController]
	public class InvoicesController : ControllerBase
	{
		private int userId => GetItemFromContext<int>("userId");
		private bool isAdmin => GetItemFromContext<bool>("isAdmin");
		private readonly IInvoiceService _service;

		private T GetItemFromContext<T>(string key)
		{
			if (HttpContext?.Items[key] is T value)
				return value;

			throw new UnauthorizedAccessException($"{key} is missing or invalid.");
		}

		public InvoicesController(IInvoiceService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<InvoicesViewDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPaged queryPaged)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				queryPaged.UserId = userId;
				queryPaged.IsAdmin = isAdmin;
				var result = await _service.GetPagedInvoicesAsync(queryPaged);
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
		public async Task<IActionResult> CreateAsync([FromBody] InvoiceEditDto invoiceDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.CreateInvoiceAsync(userId, isAdmin, invoiceDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceEditDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var result = await _service.GetInvoiceDtoByIdAsync(userId, isAdmin, id);

				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("ArchiveUnarchive")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ArchiveInvoice([FromQuery] QueryInvoiceChangeArchive queryArchive)
		{
			try
			{
				queryArchive.UserId = userId;
				queryArchive.IsAdmin = isAdmin;

				var result = await _service.ArchiveUnarchiveEntity(queryArchive);

				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("ChangeInvoiceStatus")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ArchiveInvoice([FromQuery] QueryInvoiceChangeStatus queryStatus)
		{
			try
			{
				queryStatus.UserId = userId;
				queryStatus.IsAdmin = isAdmin;

				var result = await _service.ChangeInvoiceStatus(queryStatus);

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
		public async Task<IActionResult> UpdateAsync([FromBody] InvoiceEditDto InvoiceDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.UpdateInvoiceAsync(userId, isAdmin, InvoiceDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = $"InvalidOperationException {ex.Message}" });
			}
		}

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var result = await _service.DeleteInvoiceAsync(userId, isAdmin, id);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{id:int}/{modeDelete:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteOrMarkAdDeletedAsync(int id, int modeDelete)
		{
			try
			{
				var result = await _service.DeleteOrMarkAsDeletedAsync(userId, isAdmin, id, modeDelete);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("GetInfoForNewInvoice")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoForNewInvoiceDto))]
		public async Task<IActionResult> GetInfoForNewInvoice(int invoiceId = 0)
		{
			try
			{
				var result = await _service.GetInfoForNewInvoice(userId, isAdmin, invoiceId);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
