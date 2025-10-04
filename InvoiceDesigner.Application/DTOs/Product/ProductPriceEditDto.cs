using InvoiceDesigner.Application.DTOs.Currency;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.DTOs.Product
{
	public class ProductPriceEditDto
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		[Required]
		public CurrencyAutocompleteDto Currency { get; set; } = null!;
		[Required]
		public decimal Price { get; set; }
	}
}
