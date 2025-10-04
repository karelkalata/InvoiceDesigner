﻿using InvoiceDesigner.Application.DTOs.Bank;
using InvoiceDesigner.Application.DTOs.Company;
using InvoiceDesigner.Application.DTOs.Currency;
using InvoiceDesigner.Application.DTOs.Customer;
using InvoiceDesigner.Application.DTOs.Invoice;
using InvoiceDesigner.Application.DTOs.InvoiceItem;
using InvoiceDesigner.Application.Helpers.Currency;

namespace InvoiceDesigner.Application.Helpers
{
	public class InvoiceExample
	{

		public InvoicePrintDto GetInvoiceExample()
		{
			var invoicePrintDto = new InvoicePrintDto();

			Random random = new Random();
			invoicePrintDto.Number = random.Next(10, 99999);
			invoicePrintDto.PONumber = random.Next(10, 99999).ToString();
			invoicePrintDto.DateTime = DateTime.Now;
			invoicePrintDto.DueDate = DateTime.Now.AddDays(random.Next(3, 30));
			invoicePrintDto.Vat = 21;
			invoicePrintDto.EnabledVat = true;


			invoicePrintDto.Company = new CompanyPrintDto
			{
				TaxId = "12345678",
				VatId = "CZ12345678",
				PaymentTerms = 14,
				Name = "Company s.r.o.",
				WWW = "www.company.cz",
				Email = "karelkalata@gmail.com",
				Phone = "+420 000 123 456",
				Address = "Jiráskovo nám. 1981/6, 120 00 Praha",
				Info = "additional information"
			};

			invoicePrintDto.Customer = new CustomerPrintDto
			{
				Name = "ABSD Corporation",
				TaxId = random.Next(10000000, 99999999).ToString(),
				VatId = "NL" + random.Next(10000000, 99999999)
			};

			invoicePrintDto.Currency = new CurrencyPrintDto
			{
				Name = "EUR",
				Description = "Euro",
				CurrencySymbol = CurrencyUnicodeHEX.GetCurrencyUnicodeHEX("EUR")
			};

			invoicePrintDto.Bank = new BankPrintDto
			{
				Name = "FinTech Bank",
				BIC = "FOIBCZPPXXX",
				Account = "CZ64BOFI90583812345678",
				Address = "Jiráskovo nám. 2021/1, 110 00 Praha 1"
			};

			for (int i = 0; i < 10; i++)
			{
				invoicePrintDto.InvoiceItems.Add(new InvoiceItemPrintDto
				{
					ProductName = $"Item #{i}",
					Price = random.Next(1, 9999),
					Quantity = random.Next(1, 10)
				});
			}

			invoicePrintDto.Amount = invoicePrintDto.InvoiceItems.Sum(item => item.Price * item.Quantity);
			return invoicePrintDto;

		}
	}
}
