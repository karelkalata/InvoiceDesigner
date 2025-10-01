
namespace InvoiceDesigner.Application.Commands
{
	public record DeleteEntityCommand
	{
		public int UserId { get; init; }
		public bool IsAdmin { get; init; } = false;
		public int EntityId { get; init; }
		public bool MarkAsDeleted { get; init; } = false;
	}
}
