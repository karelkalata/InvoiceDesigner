using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Services.ServiceFormDesigner
{
	public class CssStyleService : ICssStyleService
	{

		public List<CssStyle> MapCssStyle(List<CssStyleEditDto> EditDto)
		{
			var cssStyle = new List<CssStyle>();
			foreach (var item in EditDto)
			{
				cssStyle.Add(new CssStyle
				{
					Name = item.Name,
					Value = item.Value,
				});
			}
			return cssStyle;
		}

		public List<CssStyle> GetDefaultInvoiceItemsCssStyle()
		{
			var cssStyle = GetDefaultCssStyles();
			cssStyle.Add(new CssStyle { Name = ConstsCssProperty.AddCurrencySymbol, Value = ConstsCssProperty.Value_None });
			cssStyle.Add(new CssStyle { Name = ConstsCssProperty.AddCurrencySymbolFooter, Value = ConstsCssProperty.Value_None });
			cssStyle.Add(new CssStyle { Name = ConstsCssProperty.FooterLeftMargin, Value = ConstsCssProperty.FooterLeftMargin_Value_75 });
			return cssStyle;
		}

		public List<CssStyle> GetDefaultCssStyles()
		{
			return new List<CssStyle>
			{
				new CssStyle { Name = ConstsCssProperty.FlexGrow, Value = ConstsCssProperty.Value_1 },
				new CssStyle { Name = ConstsCssProperty.TextAlign, Value = ConstsCssProperty.Value_Left },
				new CssStyle { Name = ConstsCssProperty.FontSize, Value = ConstsCssProperty.Value_12px },
				new CssStyle { Name = ConstsCssProperty.Height, Value = ConstsCssProperty.Value_25px },
				new CssStyle { Name = ConstsCssProperty.FontStyle, Value = ConstsCssProperty.Value_Normal },
				new CssStyle { Name = ConstsCssProperty.FontWight, Value = ConstsCssProperty.Value_Normal },
			};
		}

		public void UpdateDefaultCssStyle(DropItem dropItem)
		{
			// maybe new css elements have been added? let's update them.
			var defaultCssStyle = GetDefaultCssStyles();
			foreach (var item in defaultCssStyle)
			{
				if (!dropItem.CssStyle.Any(e => e.Name == item.Name))
					dropItem.CssStyle.Add(item);
			}
		}

	}
}
