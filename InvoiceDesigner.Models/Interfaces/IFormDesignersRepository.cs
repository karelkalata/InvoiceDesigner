using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;

namespace InvoiceDesigner.Domain.Shared.Interfaces
{
	public interface IFormDesignersRepository
	{
		Task<IReadOnlyCollection<FormDesigner>> GetAllFormDesignersAsync();

		Task<int> CreateFormDesignerAsync(FormDesigner formDesigner);

		Task<FormDesigner?> GetFormDesignerByIdAsync(int id);

		Task<int> UpdateFormDesignerAsync(FormDesigner formDesigner);

		Task<bool> DeleteFormDesignerAsync(FormDesigner formDesigner);

		void DeleteDropItemsFromContext(DropItem dropItem);

	}
}
