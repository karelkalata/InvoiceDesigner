using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.QueryParameters;
using InvoiceDesigner.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	public class CustomersController : RESTController
	{
		private readonly ICustomerService _service;

		public CustomersController(ICustomerService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<CustomerViewDto>))]
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
		public async Task<IActionResult> CreateAsync([FromBody] CustomerEditDto customerEditDto)
		{
			try
			{
				var result = await _service.CreateAsync(UserId, customerEditDto);
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerEditDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			try
			{
				var result = await _service.GetEditDtoByIdAsync(id);
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
		public async Task<IActionResult> UpdateAsync([FromBody] CustomerEditDto customerEditDto)
		{
			try
			{
				var result = await _service.UpdateAsync(UserId, customerEditDto);
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

		[HttpGet("FilteringData")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<CustomerAutocompleteDto>))]
		public async Task<IActionResult> FilteringData(string f = "")
		{
			try
			{
				var result = await _service.FilteringData(f);
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
