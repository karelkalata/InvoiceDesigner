using InvoiceDesigner.Domain.Shared.Models.FormDesigner;

namespace InvoiceDesigner.Application.Helpers
{
	public static class DefaultDropItemStyles
	{
		public static List<DropItemCssStyle> GetDefaultStyles()
		{
			return new List<DropItemCssStyle>
			{
				new DropItemCssStyle { Name = ConstsCssProperty.FlexGrow, Value = ConstsCssProperty.Value_1 },
				new DropItemCssStyle { Name = ConstsCssProperty.TextAlign, Value = ConstsCssProperty.Value_Left },
				new DropItemCssStyle { Name = ConstsCssProperty.FontSize, Value = ConstsCssProperty.Value_12px },
				new DropItemCssStyle { Name = ConstsCssProperty.Height, Value = ConstsCssProperty.Value_25px },
			};
		}

		public static List<DropItemCssStyle> GetDefaultStylesItems()
		{
			return new List<DropItemCssStyle>
			{
				new DropItemCssStyle { Name = ConstsCssProperty.FlexGrow, Value = ConstsCssProperty.Value_1 },
				new DropItemCssStyle { Name = ConstsCssProperty.TextAlign, Value = ConstsCssProperty.Value_Left },
				new DropItemCssStyle { Name = ConstsCssProperty.FontSize, Value = ConstsCssProperty.Value_12px },
				new DropItemCssStyle { Name = ConstsCssProperty.Height, Value = ConstsCssProperty.Value_25px },
				new DropItemCssStyle { Name = ConstsCssProperty.AddCurrencySymbol, Value = ConstsCssProperty.Value_None},
				new DropItemCssStyle { Name = ConstsCssProperty.AddCurrencySymbolFooter, Value = ConstsCssProperty.Value_None},
			};
		}
	}

}
