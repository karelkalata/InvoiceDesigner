using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;

namespace InvoiceDesigner.Domain.Shared.Interfaces.Accounting
{
	public interface IAccountingRepository
	{
		Task CreateAsync(DoubleEntry entity);
		Task DeleteAsync(EAccountingDocument typeDocument, int documentId);
	}
}
