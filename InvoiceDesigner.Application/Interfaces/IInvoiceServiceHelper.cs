namespace InvoiceDesigner.Application.Interfaces
{
	public interface IInvoiceServiceHelper
	{
		Task<bool> IsCompanyUsedInInvoices(int companyId);

		Task<bool> IsBankUsedInInvoices(int bankId);

		Task<bool> IsClientUsedInInvoices(int clientId);

		Task<bool> IsCurrencyUsedInInvoices(int currencyId);

		Task<bool> IsProductUsedInInvoiceItems(int productId);

	}
}
