using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Helpers
{
	public static class DefaultDropItemStyles
	{
		public static List<CssStyle> GetDefaultStyles()
		{
			return new List<CssStyle>
			{
				new CssStyle { Name = ConstsCssProperty.FlexGrow, Value = ConstsCssProperty.Value_1 },
				new CssStyle { Name = ConstsCssProperty.TextAlign, Value = ConstsCssProperty.Value_Left },
				new CssStyle { Name = ConstsCssProperty.FontSize, Value = ConstsCssProperty.Value_12px },
				new CssStyle { Name = ConstsCssProperty.Height, Value = ConstsCssProperty.Value_25px },
			};
		}

		public static List<CssStyle> GetDefaultStylesTableItems()
		{
			return new List<CssStyle>
			{
				new CssStyle { Name = ConstsCssProperty.FlexGrow, Value = ConstsCssProperty.Value_1 },
				new CssStyle { Name = ConstsCssProperty.TextAlign, Value = ConstsCssProperty.Value_Left },
				new CssStyle { Name = ConstsCssProperty.FontSize, Value = ConstsCssProperty.Value_12px },
				new CssStyle { Name = ConstsCssProperty.Height, Value = ConstsCssProperty.Value_25px },
				new CssStyle { Name = ConstsCssProperty.AddCurrencySymbol, Value = ConstsCssProperty.Value_None},
				new CssStyle { Name = ConstsCssProperty.AddCurrencySymbolFooter, Value = ConstsCssProperty.Value_None},
				new CssStyle { Name = ConstsCssProperty.FooterLeftMargin, Value = ConstsCssProperty.FooterLeftMargin_Value_75},
			};
		}
	}
}
