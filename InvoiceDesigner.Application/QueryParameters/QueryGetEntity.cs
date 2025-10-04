using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.QueryParameters
{
	public class QueryGetEntity
	{
		[Required]
		public int EntityId { get; set; }
		public int ChildEntityId { get; set; }
		public int ParentEntityId { get; set; }
	}
}
