using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Infrastructure;

namespace InvoiceDesigner.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PrintInvoiceController : ControllerBase
	{
		private readonly IPrintInvoiceService _service;

		public PrintInvoiceController(IPrintInvoiceService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PdfFileInfo))]
		public async Task<IActionResult> Index(int id = 0, int printform = 0)
		{
			try
			{
				QuestPDF.Settings.License = LicenseType.Community;
				var pdf = await _service.CreatePDF(id, printform);
				return File(pdf.ByteArray, pdf.MimeType, pdf.FileName);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}

		}
	}
}
