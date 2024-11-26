using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Services.ServiceFormDesigner
{
	public class DropItemsService : IDropItemsService
	{
		private readonly ICssStyleService _cssStyleService;

		public DropItemsService(ICssStyleService cssStyleService)
		{
			_cssStyleService = cssStyleService;
		}

		public List<DropItem> MapDropItems(List<DropItemEditDto> EditDto)
		{
			var dropItems = new List<DropItem>();
			foreach (var item in EditDto)
			{
				if (item.Selector.StartsWith("coor_"))
				{
					dropItems.Add(new DropItem
					{
						UniqueId = item.UniqueId,
						Value = item.Value,
						Selector = item.Selector,
						StartSelector = item.StartSelector,
						CssStyle = _cssStyleService.MapCssStyle(item.CssStyleEditDto)
					});
				}
			}
			return dropItems;
		}

		public List<DropItem> CreateListDropItems(FormDesigner formDesigner)
		{

			var result = new List<DropItem>(formDesigner.DropItems);

			var printableTypes = GetTypesImplementingInterface<IPrintable>();

			foreach (var printDto in printableTypes)
			{
				if (Activator.CreateInstance(printDto) is IPrintable instance)
				{
					foreach (var property in instance.GetType().GetProperties())
					{
						var uniqueId = $"{{{instance.GetSelectorName()}.{property.Name}}}";
						var existsDropItem = result.FirstOrDefault(e => e.UniqueId == uniqueId);

						if (existsDropItem is null)
						{
							result.Add(new DropItem
							{
								UniqueId = uniqueId,
								Value = uniqueId,
								Selector = $"_{instance.GetSelectorName()}",
								StartSelector = $"_{instance.GetSelectorName()}",
								FormDesignerSchemeId = formDesigner.Id,
								CssStyle = uniqueId == "{Invoice.InvoiceItems}" ? _cssStyleService.GetDefaultInvoiceItemsCssStyle() : _cssStyleService.GetDefaultCssStyles()
							});
						}
						else
						{
							// maybe new css elements have been added? let's update them.
							_cssStyleService.UpdateDefaultCssStyle(existsDropItem);
						}
					}
				}
			}
			return result;
		}

		private List<Type> GetTypesImplementingInterface<TInterface>()
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
				.ToList();
		}

		public DropItem AddEmptyBox()
		{
			var item = new DropItem
			{
				UniqueId = Guid.NewGuid().ToString(),
				Value = "input text here",
				Selector = "_Other",
				StartSelector = "_Other",
				CssStyle = _cssStyleService.GetDefaultCssStyles()
			};

			return item;
		}

	}
}
