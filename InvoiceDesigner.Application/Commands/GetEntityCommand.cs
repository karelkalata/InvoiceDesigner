namespace InvoiceDesigner.Application.Commands
{
	public record GetEntityCommand
	{
		public int UserId { get; init; }
		public bool IsAdmin { get; init; } = false;
		public int EntityId { get; init; }
		public int ChildEntityId { get; init; }
		public int ParentEntityId { get; init; }
	}
}
