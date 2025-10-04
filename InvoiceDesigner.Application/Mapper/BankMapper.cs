using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Domain.Shared.Models.Directories;

namespace InvoiceDesigner.Application.Mapper
{
	public static class BankMapper
	{
		public static IReadOnlyCollection<BankAutocompleteDto> ToAutocompleteDto(IReadOnlyCollection<Bank> banks) => banks.Select(ToAutocompleteDto).ToList();

		public static BankAutocompleteDto ToAutocompleteDto(Bank bank)
		{
			return new BankAutocompleteDto
			{
				Id = bank.Id,
				Name = bank.Name,
				CurrencyId = bank.CurrencyId,
				CompanyId = bank.CompanyId
			};
		}

		public static BankEditDto ToEditDto(Bank bank)
		{
			return new BankEditDto
			{
				Id = bank.Id,
				Name = bank.Name,
				BIC = bank.BIC,
				Account = bank.Account,
				Address = bank.Address,
				Currency = CurrencyMapper.ToAutocompleteDto(bank.Currency),
				CompanyId = bank.CompanyId
			};
		}

		public static BankPrintDto ToPrintDto(Bank bank)
		{
			return new BankPrintDto
			{
				Name = bank.Name,
				BIC = bank.BIC,
				Account = bank.Account,
				Address = bank.Address
			};
		}
	}
}
