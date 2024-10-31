namespace InvoiceDesigner.Application.Interfaces
{
	public interface IProductServiceHelper
	{
		Task<bool> IsCurrencyUsedInProduct(int currencyId);
	}
}
