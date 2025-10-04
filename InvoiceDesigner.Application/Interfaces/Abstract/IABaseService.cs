using InvoiceDesigner.Application.Commands;
using InvoiceDesigner.Application.Responses;
using InvoiceDesigner.Domain.Shared.Interfaces.Abstract;

namespace InvoiceDesigner.Application.Interfaces.Abstract
{
	public interface IABaseService<T> where T : class, IABaseEntity
	{
		protected Task<(IReadOnlyCollection<T> Entities, int TotalCount)> GetEntitiesAndCountAsync(PagedCommand pagedCommand);
		Task<ResponseBoolean> DeleteOrMarkAsDeletedAsync(DeleteEntityCommand deleteEntityCommand);
	}
}
