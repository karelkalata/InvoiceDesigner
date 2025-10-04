namespace InvoiceDesigner.Domain.Shared.Interfaces.Abstract
{
	public interface IABaseEntity
	{
		public int Id { get; init; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsArchived { get; set; }
	}
}
