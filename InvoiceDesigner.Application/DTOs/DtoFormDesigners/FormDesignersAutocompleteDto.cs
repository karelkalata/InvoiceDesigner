using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.DTOs.DtoFormDesigners
{
	public class FormDesignersAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public EAccountingDocument AccountingDocument { get; set; }
	}
}
