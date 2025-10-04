using InvoiceDesigner.Application.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class ProductMapper
	{
		public static IReadOnlyCollection<ProductAutocompleteDto> ToAutocompleteDto(IReadOnlyCollection<Product> products) => products.Select(ToAutocompleteDto).ToList();
		public static IReadOnlyCollection<ProductsViewDto> ToViewDto(IReadOnlyCollection<Product> products) => products.Select(MapToProductsViewDto).ToList();

		public static ProductAutocompleteDto ToAutocompleteDto(Product product)
		{
			return new ProductAutocompleteDto
			{
				Id = product.Id,
				Name = product.Name,
				PriceByCurrency = product.ProductPrice
					.ToDictionary(pp => pp.CurrencyId, pp => pp.Price)
			};
		}

		public static ProductsViewDto MapToProductsViewDto(Product product)
		{
			return new ProductsViewDto
			{
				Id = product.Id,
				Name = product.Name,
				IsDeleted = product.IsDeleted
			};
		}

		public static ProductEditDto ToEditDto(Product product)
		{
			return new ProductEditDto
			{
				Id = product.Id,
				Name = product.Name,
				ProductPrice = product.ProductPrice
					.Select(ProductPriceMapper.ToEditDto)
					.ToList()
			};
		}
	}
}
