namespace InvoiceDesigner.Application.Interfaces
{
	public interface ICompanyServiceHelper
	{
		Task<bool> IsCurrencyUsedInCompany(int currencyId);
	}
}
