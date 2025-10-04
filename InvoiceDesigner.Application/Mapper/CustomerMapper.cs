using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class CustomerMapper
	{
		public static IReadOnlyCollection<CustomerAutocompleteDto> ToAutocompleteDto(IReadOnlyCollection<Customer> customers) => customers.Select(ToAutocompleteDto).ToList();
		public static IReadOnlyCollection<CustomerViewDto> ToViewDto(IReadOnlyCollection<Customer> customers) => customers.Select(ToViewDto).ToList();

		public static CustomerAutocompleteDto ToAutocompleteDto(Customer customer)
		{
			return new CustomerAutocompleteDto
			{
				Id = customer.Id,
				Name = customer.Name
			};
		}

		public static CustomerViewDto ToViewDto(Customer customer)
		{
			return new CustomerViewDto
			{
				Id = customer.Id,
				Name = customer.Name,
				IsDeleted = customer.IsDeleted,
				TaxId = customer.TaxId ?? string.Empty
			};
		}

		public static CustomerPrintDto ToPrintDto(Customer customer)
		{
			return new CustomerPrintDto
			{
				Name = customer.Name,
				TaxId = customer.TaxId ?? string.Empty,
				VatId = customer.VatId ?? string.Empty
			};
		}

		public static CustomerEditDto ToEditDto(Customer customer)
		{
			return new CustomerEditDto
			{
				Id = customer.Id,
				Name = customer.Name,
				TaxId = customer.TaxId ?? string.Empty,
				VatId = customer.VatId ?? string.Empty
			};
		}
	}
}
