using AutoMapper;
using InvoiceDesigner.Domain.Shared.DTOs.InvoiceItem;
using InvoiceDesigner.Domain.Shared.Models;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperInvoiceItem : Profile
	{
		public MapperInvoiceItem()
		{

			CreateMap<InvoiceItem, InvoiceItemDto>();

			CreateMap<InvoiceItem, InvoiceItemPrintDto>().ForMember(
					dest => dest.ProductName,
					opt => opt.MapFrom(src => src.Product.Name)
				);

		}
	}
}
