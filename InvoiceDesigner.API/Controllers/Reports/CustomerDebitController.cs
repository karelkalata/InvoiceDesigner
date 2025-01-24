using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.Interfaces.Reports;
using InvoiceDesigner.Domain.Shared.DTOs.Reports.CustomerDebit;
using InvoiceDesigner.Domain.Shared.QueryParameters.Report;
using InvoiceDesigner.Domain.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.Reports
{
	[Route("api/Reports/[controller]")]
	public class CustomerDebitController : RESTController
	{
		private readonly ICustomerDebitService _service;

		public CustomerDebitController(ICustomerDebitService service)
		{
			_service = service;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponsePaged<ReportCustomerDebit>))]
		public async Task<IActionResult> Index([FromQuery] QueryCustomerDebit queryPaged)
		{
			try
			{
				queryPaged.UserId = UserId;
				queryPaged.IsAdmin = IsAdmin;
				var result = await _service.GetPagedAsync(queryPaged);
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
