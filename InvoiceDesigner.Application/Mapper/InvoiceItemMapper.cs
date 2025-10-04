using InvoiceDesigner.Application.DTOs.InvoiceItem;
using InvoiceDesigner.Domain.Shared.Models.Documents;

namespace InvoiceDesigner.Application.Mapper
{
	public static class InvoiceItemMapper
	{
		public static InvoiceItemDto ToEditDto(InvoiceItem item)
		{
			return new InvoiceItemDto
			{
				Id = item.Id,
				Item = ProductMapper.ToAutocompleteDto(item.Item),
				Price = item.Price,
				Quantity = item.Quantity
			};
		}

		public static InvoiceItemPrintDto ToPrintDto(InvoiceItem item)
		{
			return new InvoiceItemPrintDto
			{
				ProductName = item.Item.Name,
				Price = item.Price,
				Quantity = item.Quantity
			};
		}
	}
}
