using InvoiceDesigner.Application.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner
{
	public interface ICssStyleService
	{
		List<CssStyle> MapCssStyle(List<CssStyleEditDto> EditDto);

		List<CssStyle> GetDefaultInvoiceItemsCssStyle();

		List<CssStyle> GetDefaultCssStyles();

		void UpdateDefaultCssStyle(DropItem dropItem);

	}
}
