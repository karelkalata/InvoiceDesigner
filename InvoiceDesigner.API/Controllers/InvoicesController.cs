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
	[Route("api/[controller]")]
	[ApiController]
	public class InvoicesController : ControllerBase
	{
		private readonly IInvoiceService _service;

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
				var (userId, isAdmin) = GetValidatedFilters();
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
				var (userId, isAdmin) = GetValidatedFilters();
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
				var (userId, isAdmin) = GetValidatedFilters();
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
				var (userId, isAdmin) = GetValidatedFilters();

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
				var (userId, isAdmin) = GetValidatedFilters();

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
				var (userId, isAdmin) = GetValidatedFilters();
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
				var (userId, isAdmin) = GetValidatedFilters();
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
				var (userId, isAdmin) = GetValidatedFilters();
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
				var (userId, isAdmin) = GetValidatedFilters();
				var result = await _service.GetInfoForNewInvoice(userId, isAdmin, invoiceId);
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
