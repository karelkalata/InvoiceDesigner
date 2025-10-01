using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.DTOs.Customer
{
	public class CustomerAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

	}
}
