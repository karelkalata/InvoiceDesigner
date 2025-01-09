using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Application.Interfaces.InterfacesAccounting
{
	public interface IAccountingService
	{
		Task CreateEntriesAsync(AAccountingDocument document, EAccountingDocument typeDocument, EStatus status);

	}
}
