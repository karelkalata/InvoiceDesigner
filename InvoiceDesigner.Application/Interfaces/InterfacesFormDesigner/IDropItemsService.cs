using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner
{
	public interface IDropItemsService
	{
		List<DropItem> MapDropItems(List<DropItemEditDto> EditDto);

		List<DropItem> CreateListDropItems(FormDesigner formDesigner);

		DropItem AddEmptyBox();

	}
}
