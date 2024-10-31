using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Domain.Shared.DTOs.Product
{
	public class ProductAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		//public ICollection<ProductPrice> ProductPrice { get; set; } = new List<ProductPrice>();

		public Dictionary<int, decimal> PriceByCurrency { get; set; } = new Dictionary<int, decimal>();
	}
}
