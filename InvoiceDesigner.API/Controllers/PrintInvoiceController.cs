using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;

namespace InvoiceDesigner.API.Controllers
{
	public class PrintInvoiceController : RESTController
	{
		private readonly IPrintInvoiceService _service;

		public PrintInvoiceController(IPrintInvoiceService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePdfGuid))]
		public async Task<IActionResult> GenerateDownloadLink(int id = 0, int PrintFormId = 0)
		{
			try
			{
				var result = await _service.GenerateDownloadLink(id, PrintFormId);
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

		[AllowAnonymous]
		[HttpGet("{guid}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePdf))]
		public async Task<IActionResult> CreatePDF(Guid guid)
		{
			try
			{
				QuestPDF.Settings.License = LicenseType.Community;
				var pdf = await _service.CreatePDF(guid);
				return File(pdf.ByteArray, pdf.MimeType, pdf.FileName);
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
