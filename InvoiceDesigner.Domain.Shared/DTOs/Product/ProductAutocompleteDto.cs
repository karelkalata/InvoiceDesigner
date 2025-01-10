using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Domain.Shared.DTOs.Product
{
	public class ProductAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public Dictionary<int, decimal> PriceByCurrency { get; set; } = new Dictionary<int, decimal>();
	}
}
