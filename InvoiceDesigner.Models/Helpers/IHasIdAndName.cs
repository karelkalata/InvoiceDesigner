namespace InvoiceDesigner.Domain.Shared.Helpers
{
	public interface IHasIdAndName
	{
		int Id { get; set; }
		string Name { get; set; }
	}
}
