using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Domain.Shared.DTOs.Product
{
	public class ProductEditDto
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(200, MinimumLength = 1, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
		public string Name { get; set; } = string.Empty;

		public ICollection<ProductPriceEditDto> ProductPrice { get; set; } = new List<ProductPriceEditDto>();
	}
}
