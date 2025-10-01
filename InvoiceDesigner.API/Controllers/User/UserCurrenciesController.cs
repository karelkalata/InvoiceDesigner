using InvoiceDesigner.API.Controllers.Abstract;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceDesigner.API.Controllers.User
{
	public class UserCurrenciesController : RESTController
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
