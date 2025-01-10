using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class Product : ABaseEntity
	{
		public ICollection<ProductPrice> ProductPrice { get; set; } = new List<ProductPrice>();
	}
}
