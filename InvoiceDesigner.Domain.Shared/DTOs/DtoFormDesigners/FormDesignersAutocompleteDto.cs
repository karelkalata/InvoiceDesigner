using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.DtoFormDesigners
{
	public class FormDesignersAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public EAccountingDocument AccountingDocument { get; set; }
	}
}
