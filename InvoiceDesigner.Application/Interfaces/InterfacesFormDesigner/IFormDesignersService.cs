using InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using InvoiceDesigner.Domain.Shared.Responses;

namespace InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner
{
	public interface IFormDesignersService
	{
		Task<IReadOnlyCollection<FormDesignersAutocompleteDto>> GetAllFormDesignersAutocompleteDto();

		Task<ResponseRedirect> CreateFormDesignerAsync(FormDesignerEditDto formDesignerEditDto);

		Task<FormDesigner> GetFormDesignerByIdAsync(int id);

		Task<FormDesignerEditDto> GetFormDesignerEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateFormDesignerAsync(FormDesignerEditDto formDesignerEditDto);

		Task<ResponseBoolean> DeleteFormDesignerAsync(int id);

		DropItemEditDto AddEmptyBox();
	}
}
