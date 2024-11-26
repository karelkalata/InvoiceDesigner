namespace InvoiceDesigner.Domain.Shared.DTOs.User
{
	public class UserViewDto
	{
		public int Id { get; set; }

		public string Login { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public bool IsAdmin { get; set; }

	}
}
