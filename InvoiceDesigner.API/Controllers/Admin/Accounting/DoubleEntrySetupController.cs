using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Domain.Shared.DTOs.AccountingDTOs;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Admin.Accounting
{
	[Authorize(Policy = UserPolicy.IsAdmin)]
	[Route("api/Admin/Accounting/[controller]")]
	public class DoubleEntrySetupController : RESTController
	{
		private readonly IDoubleEntrySetupService _serviceDoubleEntrySetup;

		public DoubleEntrySetupController(IDoubleEntrySetupService doubleEntrySetupService)
		{
			_serviceDoubleEntrySetup = doubleEntrySetupService;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<DoubleEntrySetupEditDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPagedDoubleEntrySetup queryPaged)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			try
			{
				var result = await _serviceDoubleEntrySetup.GetPagedAsync(queryPaged);
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
		public async Task<IActionResult> CreateAsync([FromBody] DoubleEntrySetupEditDto createDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _serviceDoubleEntrySetup.CreateAsync(createDto);
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
		public async Task<IActionResult> UpdateAsync([FromBody] DoubleEntrySetupEditDto editedDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _serviceDoubleEntrySetup.UpdateAsync(editedDto);
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
		public async Task<IActionResult> DeleteOrMarkAdDeletedAsync(int id)
		{
			try
			{
				var result = await _serviceDoubleEntrySetup.DeleteAsync(id);
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
