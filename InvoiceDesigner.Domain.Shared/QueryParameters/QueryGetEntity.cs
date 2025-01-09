namespace InvoiceDesigner.Domain.Shared.QueryParameters
{
	public class QueryGetEntity
	{
		public int UserId { get; set; }

		public bool IsAdmin { get; set; } = false;

		public int EntityId { get; set; }

		public int ChildEntityId { get; set; }

		public int ParentEntityId { get; set; }
	}
}
