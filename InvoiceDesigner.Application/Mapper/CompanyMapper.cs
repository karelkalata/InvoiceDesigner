using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class CompanyMapper
	{
		public static IReadOnlyCollection<CompanyAutocompleteDto> ToAutocompleteDto(IReadOnlyCollection<Company> companies) => companies.Select(ToAutocompleteDto).ToList();
		public static IReadOnlyCollection<CompanyViewDto> ToViewDto(IReadOnlyCollection<Company> companies) => companies.Select(ToViewDto).ToList();

		public static CompanyViewDto ToViewDto(Company company)
		{
			return new CompanyViewDto
			{
				Id = company.Id,
				Name = company.Name,
				IsDeleted = company.IsDeleted,
				TaxId = company.TaxId ?? string.Empty,
				CurrencyName = company.Currency?.Name ?? string.Empty
			};
		}


		public static CompanyAutocompleteDto ToAutocompleteDto(Company company)
		{
			return new CompanyAutocompleteDto
			{
				Id = company.Id,
				Name = company.Name,
				PaymentTerms = company.PaymentTerms,
				DefaultVat = company.DefaultVat,
				Currency = CurrencyMapper.ToAutocompleteDto(company.Currency),
				Banks = company.Banks.Select(BankMapper.ToAutocompleteDto).ToList()
			};
		}


		public static CompanyEditDto ToEditDto(Company company)
		{
			return new CompanyEditDto
			{
				Id = company.Id,
				Name = company.Name,
				TaxId = company.TaxId,
				VatId = company.VatId,
				WWW = company.WWW,
				Email = company.Email,
				Phone = company.Phone,
				Address = company.Address,
				Info = company.Info,
				PaymentTerms = company.PaymentTerms,
				DefaultVat = company.DefaultVat,
				Currency = CurrencyMapper.ToAutocompleteDto(company.Currency),
				Banks = company.Banks.Select(BankMapper.ToEditDto).ToList()
			};
		}

		public static CompanyPrintDto ToPrintDto(Company company)
		{
			return new CompanyPrintDto
			{
				Name = company.Name,
				TaxId = company.TaxId,
				VatId = company.VatId ?? string.Empty,
				WWW = company.WWW ?? string.Empty,
				Email = company.Email ?? string.Empty,
				Phone = company.Phone ?? string.Empty,
				Address = company.Address ?? string.Empty,
				Info = company.Info ?? string.Empty,
				PaymentTerms = company.PaymentTerms
			};
		}
	}

}
