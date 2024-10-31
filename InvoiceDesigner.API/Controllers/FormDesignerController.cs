using AutoMapper;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.FormDesigners;
using InvoiceDesigner.Domain.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FormDesignerController : Controller
	{
		private readonly IFormDesignersService _service;
		private readonly IMapper _mapper;

		public FormDesignerController(IFormDesignersService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}


		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody] FormDesignerEditDto formDesignerEditDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.CreateFormDesignerAsync(formDesignerEditDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FormDesignerEditDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			try
			{
				var result = await _service.GetFormDesignerEditDtoByIdAsync(id);

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
		public async Task<IActionResult> UpdateAsync([FromBody] FormDesignerEditDto formDesignerEditDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.UpdateFormDesignerAsync(formDesignerEditDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			try
			{
				var result = await _service.DeleteFormDesignerAsync(id);

				if (!result)
					return BadRequest(new { message = "Error DeleteAsync" });

				return NoContent();
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpGet("GetAllFormDesignersAutocompleteDto")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<FormDesignersAutocompleteDto>))]
		public async Task<IActionResult> GetAllDesignersAutocompleteDto()
		{
			var result = await _service.GetAllFormDesignersAutocompleteDto();
			return Ok(result);
		}


		[HttpGet("AddEmptyBox")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropItemEditDto))]
		public IActionResult AddEmptyBox()
		{
			var result = _service.AddEmptyBox();
			return Ok(result);
		}

	}
}
