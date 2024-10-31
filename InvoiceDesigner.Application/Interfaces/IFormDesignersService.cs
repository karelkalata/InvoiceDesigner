using InvoiceDesigner.Domain.Shared.DTOs.Company;
using InvoiceDesigner.Domain.Shared.DTOs.FormDesigners;
using InvoiceDesigner.Domain.Shared.Helpers;
using InvoiceDesigner.Domain.Shared.Models.FormDesigner;

namespace InvoiceDesigner.Application.Interfaces
{
	public interface IFormDesignersService
	{
		Task<IReadOnlyCollection<FormDesignersAutocompleteDto>> GetAllFormDesignersAutocompleteDto();

		Task<ResponseRedirect> CreateFormDesignerAsync(FormDesignerEditDto formDesignerEditDto);
		
		Task<FormDesigner> GetFormDesignerByIdAsync(int id);

		Task<FormDesignerEditDto> GetFormDesignerEditDtoByIdAsync(int id);

		Task<ResponseRedirect> UpdateFormDesignerAsync(FormDesignerEditDto formDesignerEditDto);

		Task<bool> DeleteFormDesignerAsync(int id);

		DropItemEditDto AddEmptyBox();

	}
}
