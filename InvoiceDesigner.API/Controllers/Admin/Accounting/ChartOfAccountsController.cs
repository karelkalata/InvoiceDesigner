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

	[Route("api/Admin/Accounting/[controller]")]
	public class ChartOfAccountsController : RESTController
	{
		private readonly IChartOfAccountsService _serviceChartOfAccounts;

		public ChartOfAccountsController(IChartOfAccountsService serviceChartOfAccounts)
		{
			_serviceChartOfAccounts = serviceChartOfAccounts;
		}

		[HttpGet]
		[Authorize(Policy = UserPolicy.IsAdmin)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<ChartOfAccountsDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPaged queryPaged)
		{
			try
			{
				var result = await _serviceChartOfAccounts.GetPagedAsync(queryPaged);
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
		[Authorize(Policy = UserPolicy.IsAdmin)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody] ChartOfAccountsDto createDto)
		{
			try
			{
				var result = await _serviceChartOfAccounts.CreateAsync(createDto);
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
		[Authorize(Policy = UserPolicy.IsAdmin)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody] ChartOfAccountsDto editedDto)
		{
			try
			{
				var result = await _serviceChartOfAccounts.UpdateAsync(editedDto);
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
		[Authorize(Policy = UserPolicy.IsAdmin)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteOrMarkAsDeletedAsync(int id)
		{
			try
			{
				var result = await _serviceChartOfAccounts.DeleteAsync(id);
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


		[HttpGet("FilteringData")]
		[Authorize]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<ChartOfAccountsAutocompleteDto>))]
		public async Task<IActionResult> FilteringData(string f = "")
		{
			try
			{
				var result = await _serviceChartOfAccounts.FilteringData(f);
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
