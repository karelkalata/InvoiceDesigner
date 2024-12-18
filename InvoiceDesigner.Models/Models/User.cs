namespace InvoiceDesigner.Domain.Shared.Models
{
	public class User
	{
		public int Id { get; init; }

		public string Login { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public string Locale { get; set; } = "en-US";

		public bool IsDeleted { get; set; }

		public bool IsAdmin { get; set; } = false;

		public string PasswordHash { get; set; } = string.Empty;

		public string PasswordSalt { get; set; } = string.Empty;

		public ICollection<Company> Companies { get; set; } = new List<Company>();

	}
}
