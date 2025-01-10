using InvoiceDesigner.Domain.Shared.Models.Abstract;

namespace InvoiceDesigner.Domain.Shared.Models.Directories
{
	public class User : ABaseEntity
	{
		public string Login { get; set; } = string.Empty;
		public string Locale { get; set; } = "en-US";
		public bool IsAdmin { get; set; } = false;
		public string PasswordHash { get; set; } = string.Empty;
		public string PasswordSalt { get; set; } = string.Empty;
		public ICollection<Company> Companies { get; set; } = new List<Company>();
	}
}
