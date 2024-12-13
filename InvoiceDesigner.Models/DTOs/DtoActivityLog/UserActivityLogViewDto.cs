using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.DTOs.DtoActivityLog
{
	public class UserActivityLogViewDto
	{
		public DateTime DateTime { get; set; }

		public string UserName { get; set; } = string.Empty;

		public EActivitiesType ActivitiesType { get; set; }

		public EDocumentsTypes DocumentTypes { get; set; }

		public int? EntityId { get; set; }

		public string? EntityNumber { get; set; }
	}
}
