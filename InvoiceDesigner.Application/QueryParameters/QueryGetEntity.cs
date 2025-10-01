namespace InvoiceDesigner.Application.QueryParameters
{
	public class QueryGetEntity
	{
		public int EntityId { get; set; }
		public int ChildEntityId { get; set; }
		public int ParentEntityId { get; set; }
	}
}
