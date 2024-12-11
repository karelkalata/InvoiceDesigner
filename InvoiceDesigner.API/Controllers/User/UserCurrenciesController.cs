using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.User
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserCurrenciesController : ControllerBase
	{
		private readonly ICurrencyService _serviceCurrency;

		public UserCurrenciesController(ICurrencyService serviceCurrency)
		{
			_serviceCurrency = serviceCurrency;
		}

		[HttpGet("FilteringData")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<CurrencyAutocompleteDto>))]
		public async Task<IActionResult> FilteringData(string f = "")
		{
			var result = await _serviceCurrency.FilteringData(f);
			return Ok(result);
		}
	}
}
