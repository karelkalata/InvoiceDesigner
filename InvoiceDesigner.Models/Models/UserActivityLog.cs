using InvoiceDesigner.Domain.Shared.Enums;

namespace InvoiceDesigner.Domain.Shared.Models
{
	public class UserActivityLog
	{
		public int Id { get; init; }

		public DateTime DateTime { get; set; } = DateTime.Now;

		public int UserId { get; set; }

		public User User { get; set; } = null!;

		public EDocumentsTypes DocumentTypes { get; set; }

		public int? EntityId { get; set; }

		public string? EntityNumber { get; set; }

		public EActivitiesType ActivitiesType { get; set; }

	}
}
