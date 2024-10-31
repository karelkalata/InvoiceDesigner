using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.Product;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperProduct : Profile
	{
		public MapperProduct()
		{
			CreateMap<Product, ProductAutocompleteDto>()
				.ForMember(
					dest => dest.PriceByCurrency, 
					opt => opt.MapFrom(
						src => src.ProductPrice.ToDictionary(
							e => e.CurrencyId, 
							e => e.Price
						)
					)
				);

			CreateMap<Product, ProductEditDto>();

			CreateMap<Product, ProductsViewDto>();

			CreateMap<ProductPrice, ProductPriceEditDto>();

		}
	}
}
