using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.DTOs;
using InvoiceDesigner.Domain.Shared.DTOs.InvoiceDTOs;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	public class InvoicesController : RESTController
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
				queryPaged.UserId = UserId;
				queryPaged.IsAdmin = IsAdmin;
				var result = await _service.GetPagedAsync(queryPaged);
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
				var result = await _service.CreateAsync(UserId, IsAdmin, invoiceDto);
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceEditDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var result = await _service.GetDtoByIdAsync(UserId, IsAdmin, id);
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

		[HttpGet("InvoiceChangeProperty")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> InvoiceChangeProperty([FromQuery] QueryOnChangeProperty query)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				query.UserId = UserId;
				query.IsAdmin = IsAdmin;

				var result = await _service.OnChangeProperty(query);
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
		public async Task<IActionResult> UpdateAsync([FromBody] InvoiceEditDto InvoiceDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.UpdateAsync(UserId, IsAdmin, InvoiceDto);
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

		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var result = await _service.DeleteAsync(UserId, IsAdmin, id);
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
		public async Task<IActionResult> DeleteOrMarkAdDeletedAsync(int id, int modeDelete)
		{
			try
			{
				var result = await _service.DeleteOrMarkAsDeletedAsync(UserId, IsAdmin, id, modeDelete);
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

		[HttpGet("GetInfoForNewInvoice")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoForNewInvoiceDto))]
		public async Task<IActionResult> GetInfoForNewInvoice(int invoiceId = 0)
		{
			try
			{
				var result = await _service.GetInfoForNewInvoice(UserId, IsAdmin, invoiceId);
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
