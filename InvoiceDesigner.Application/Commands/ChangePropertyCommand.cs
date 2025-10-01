using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Application.Commands
{
	public record ChangePropertyCommand
	{
		public int UserId { get; init; }
		public bool IsAdmin { get; init; } = false;
		public int EntityId { get; init; }
		public bool IsDeleted { get; init; }
		public bool IsArchived { get; init; }
		public EStatus Status { get; init; }
	}
}
