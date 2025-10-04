using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs;
using InvoiceDesigner.Application.DTOs.Invoice;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Application.QueryParameters;
using InvoiceDesigner.Application.Responses;
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
			var pagedCommand = new PagedCommand
			{
				UserId = UserId,
				IsAdmin = IsAdmin,
				PageSize = queryPaged.PageSize,
				Page = queryPaged.Page,
				SearchString = queryPaged.SearchString,
				SortLabel = queryPaged.SortLabel,
				ShowDeleted = queryPaged.ShowDeleted,
				ShowArchived = queryPaged.ShowArchived,
			};

			var result = await _service.GetPagedEntitiesAsync(pagedCommand);
			return Ok(result);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody] InvoiceEditDto invoiceDto)
		{
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
		public async Task<IActionResult> InvoiceChangeProperty([FromQuery] QueryChangeProperty queryChangeProperty)
		{
			var changePropertyCommand = new ChangePropertyCommand
			{
				UserId = UserId,
				IsAdmin = IsAdmin,
				EntityId = queryChangeProperty.EntityId,
				IsDeleted = queryChangeProperty.IsDeleted,
				IsArchived = queryChangeProperty.IsArchived,
				Status = queryChangeProperty.Status,
			};
			var result = await _service.OnChangeProperty(changePropertyCommand);
			return Ok(result);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody] InvoiceEditDto InvoiceDto)
		{
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
		public async Task<IActionResult> DeleteOrMarkAdDeletedAsync(int id, int modeDelete)
		{
			var deleteEntityCommand = new DeleteEntityCommand
			{
				UserId = UserId,
				IsAdmin = IsAdmin,
				EntityId = id,
				MarkAsDeleted = modeDelete == 0
			};
			return Ok(await _service.DeleteOrMarkAsDeletedAsync(deleteEntityCommand));
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
