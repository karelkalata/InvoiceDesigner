using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Product;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _service;

		public ProductsController(IProductService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<ProductsViewDto>))]
		public async Task<IActionResult> Index([FromQuery] QueryPaged queryPaged)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.GetPagedProductsAsync(queryPaged);
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
		public async Task<IActionResult> CreateAsync([FromBody] ProductEditDto productEditDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.CreateProductAsync(productEditDto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductEditDto))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			try
			{
				var result = await _service.GetProductEditDtoByIdAsync(id);

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
		public async Task<IActionResult> UpdateAsync([FromBody] ProductEditDto productEditDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _service.UpdateProductAsync(productEditDto);
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
				var result = await _service.DeleteProductAsync(id);
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
				var result = await _service.DeleteOrMarkAsDeletedAsync(id, modeDelete);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("FilteringData")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<ProductAutocompleteDto>))]
		public async Task<IActionResult> FilteringData(string f = "")
		{
			var result = await _service.FilteringData(f);
			return Ok(result);
		}
	}
}
