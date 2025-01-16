using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.DTOs.BankReceiptDTOs;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	public class BankReceiptsController : RESTController
	{
		private readonly IBankReceiptService _service;

		public BankReceiptsController(IBankReceiptService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<BankReceiptViewDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPaged queryPaged)
		{
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
		public async Task<IActionResult> CreateAsync([FromBody] BankReceiptCreateDto editedDto)
		{
			try
			{
				var result = await _service.CreateAsync(UserId, IsAdmin, editedDto);
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BankReceiptViewDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetById(int id, [FromQuery] QueryGetEntity queryGetEntity)
		{
			try
			{
				queryGetEntity.UserId = UserId;
				queryGetEntity.IsAdmin = IsAdmin;
				var result = await _service.GetDtoByIdAsync(queryGetEntity);
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
		public async Task<IActionResult> UpdateAsync([FromBody] BankReceiptCreateDto editedDto)
		{
			try
			{
				var result = await _service.UpdateAsync(UserId, IsAdmin, editedDto);
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
				var queryDeleteEntity = new QueryDeleteEntity
				{
					UserId = UserId,
					IsAdmin = IsAdmin,
					EntityId = id,
					MarkAsDeleted = modeDelete == 0
				};

				var result = await _service.DeleteOrMarkAsDeletedAsync(queryDeleteEntity);
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

		[HttpGet("OnChangeProperty")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> OnChangeProperty([FromQuery] QueryOnChangeProperty query)
		{
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
	}
}
