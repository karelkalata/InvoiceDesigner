using AutoMapper;
using InvoiceDesigner.Application.DTOs.InvoiceItem;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Mapper
{
	public class MapperInvoiceItem : Profile
	{
		public MapperInvoiceItem()
		{

			CreateMap<InvoiceItem, InvoiceItemDto>();

			CreateMap<InvoiceItem, InvoiceItemPrintDto>().ForMember(
					dest => dest.ProductName,
					opt => opt.MapFrom(src => src.Item.Name)
				);
		}
	}
}
