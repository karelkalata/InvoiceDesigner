using InvoiceDesigner.Application.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class ProductPriceMapper
	{
		public static ProductPriceEditDto ToEditDto(ProductPrice price)
		{
			return new ProductPriceEditDto
			{
				Id = price.Id,
				ItemId = price.ProductId,
				Currency = CurrencyMapper.ToAutocompleteDto(price.Currency),
				Price = price.Price
			};
		}
	}
}
