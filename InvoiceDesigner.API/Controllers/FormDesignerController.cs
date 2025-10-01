﻿using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers
{
	public class FormDesignerController : RESTController
	{
		private readonly IFormDesignersService _service;

		public FormDesignerController(IFormDesignersService service)
		{
			_service = service;
		}


		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateAsync([FromBody] FormDesignerEditDto editDto)
		{
			try
			{
				var result = await _service.CreateAsync(UserId, editDto);
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
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}


		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseRedirect))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> UpdateAsync([FromBody] FormDesignerEditDto formDesignerEditDto)
		{
			try
			{
				var result = await _service.UpdateAsync(UserId, formDesignerEditDto);
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
		public async Task<IActionResult> DeleteAsync(int id)
		{
			try
			{
				var result = await _service.DeleteAsync(UserId, id);
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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<FormDesignersAutocompleteDto>))]
		public async Task<IActionResult> GetAllAutocompleteDto([FromQuery] EAccountingDocument typeDocument)
		{
			try
			{
				var result = await _service.GetAllAutocompleteDto(typeDocument);
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
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}

		[HttpGet("FilteringData")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<FormDesignersAutocompleteDto>))]
		public async Task<IActionResult> FilteringData([FromQuery] QueryFiltering queryFilter)
		{
			try
			{
				var result = await _service.FilteringData(queryFilter);
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
