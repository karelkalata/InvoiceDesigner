﻿using InvoiceDesigner.Domain.Shared.DTOs.Currency;
using InvoiceDesigner.Domain.Shared.Helpers;

namespace InvoiceDesigner.Domain.Shared.DTOs.Company
{
	public class CompanyAutocompleteDto : IHasIdAndName
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int PaymentTerms { get; set; }

		public CurrencyAutocompleteDto Currency { get; set; } = null!;

		public decimal DefaultVat { get; set; } = 21;
	}
}