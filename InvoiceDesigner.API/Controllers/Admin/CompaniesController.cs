using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Admin
{
	[Authorize(Policy = UserPolicy.IsAdmin)]
	public class CompaniesController : RESTController
	{
		private readonly ICompanyService _service;

		public CompaniesController(ICompanyService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<CompanyViewDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPaged queryPaged)
		{
			try
			{
				var result = await _service.GetPagedEntitiesAsync(queryPaged);
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
		public async Task<IActionResult> CreateAsync([FromBody] CompanyEditDto companyCreateDto)
		{
			try
			{
				var result = await _service.CreateAsync(UserId, companyCreateDto);
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyEditDto))]
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
		public async Task<IActionResult> UpdateAsync([FromBody] CompanyEditDto companyCreateDto)
		{
			try
			{
				var result = await _service.UpdateAsync(UserId, companyCreateDto);
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

		[HttpGet("FilteringData")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<CompanyAutocompleteDto>))]
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

		[HttpGet("GetAllAutocompleteDto")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<CompanyAutocompleteDto>))]
		public async Task<IActionResult> GetAllCompanyAutocompleteDto()
		{
			try
			{
				var result = await _service.GetAllAutocompleteDto(UserId, IsAdmin);
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