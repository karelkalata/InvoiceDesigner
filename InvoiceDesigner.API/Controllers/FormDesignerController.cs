using AutoMapper;
using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	[Authorize]
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
		public async Task<IActionResult> CreateAsync([FromBody] FormDesignerEditDto editDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var (userId, isAdmin) = GetValidatedFilters();
				var result = await _service.CreateAsync(userId, editDto);
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
				var result = await _service.GetEditDtoByIdAsync(id);

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
				var (userId, isAdmin) = GetValidatedFilters();
				var result = await _service.UpdateAsync(userId, formDesignerEditDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpDelete("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBoolean))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			try
			{
				var (userId, isAdmin) = GetValidatedFilters();
				var result = await _service.DeleteAsync(userId, id);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpGet("GetAllAutocompleteDto")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<FormDesignersAutocompleteDto>))]
		public async Task<IActionResult> GetAllAutocompleteDto()
		{
			try
			{
				var result = await _service.GetAllAutocompleteDto();
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpGet("AddEmptyBox")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DropItemEditDto))]
		public IActionResult AddEmptyBox()
		{
			try
			{
				var result = _service.AddEmptyBox();
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
