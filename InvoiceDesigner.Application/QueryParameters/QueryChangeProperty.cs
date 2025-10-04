using InvoiceDesigner.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceDesigner.Application.QueryParameters
{
	public class QueryChangeProperty
	{
		[Required]
		public int EntityId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
		[Required]
		public EStatus Status { get; set; }
	}
}
