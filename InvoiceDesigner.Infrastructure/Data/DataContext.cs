using InvoiceDesigner.Domain.Shared.Enums;
using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace InvoiceDesigner.Infrastructure.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<Company> Companies { get; set; } = null!;
		public DbSet<Currency> Currencies { get; set; } = null!;
		public DbSet<Bank> Banks { get; set; } = null!;
		public DbSet<Customer> Customers { get; set; } = null!;
		public DbSet<Product> Products { get; set; } = null!;
		public DbSet<ProductPrice> ProductPrice { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<PrintInvoice> PrintInvoices { get; set; } = null!;

		#region FormDesigners
		public DbSet<FormDesigner> FormDesigners { get; set; } = null!;
		public DbSet<FormDesignerScheme> FormDesignerSchemes { get; set; } = null!;
		public DbSet<DropItem> DropItems { get; set; } = null!;
		public DbSet<CssStyle> DropItemStyles { get; set; } = null!;
		#endregion

		#region Documents
		public DbSet<BankReceipt> BankReceipts { get; set; } = null!;
		public DbSet<Invoice> Invoices { get; set; } = null!;
		public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;
		#endregion

		#region Real Accounting
		public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; } = null!;
		public DbSet<DoubleEntrySetup> DoubleEntriesSetup { get; set; } = null!;
		public DbSet<DoubleEntry> Accounting { get; set; } = null!;
		#endregion

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			var dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


			var FormDesignerEntities = ReadJsonFile<FormDesigner>(Path.Combine(dataDirectory, "FormDesigners.json"));
			modelBuilder.Entity<FormDesigner>().HasData(FormDesignerEntities);

			var FormDesignerSchemeEntities = ReadJsonFile<FormDesignerScheme>(Path.Combine(dataDirectory, "FormDesignerSchemes.json"));
			modelBuilder.Entity<FormDesignerScheme>().HasData(FormDesignerSchemeEntities);

			var DropItemEntities = ReadJsonFile<DropItem>(Path.Combine(dataDirectory, "DropItems.json"));
			modelBuilder.Entity<DropItem>().HasData(DropItemEntities);

			var DropItemStylesEntities = ReadJsonFile<CssStyle>(Path.Combine(dataDirectory, "DropItemStyles.json"));
			modelBuilder.Entity<CssStyle>().HasData(DropItemStylesEntities);

			var ChartOfAccountsData = ReadJsonFile<ChartOfAccounts>(Path.Combine(dataDirectory, "ChartOfAccountsData.json"));
			modelBuilder.Entity<ChartOfAccounts>().HasData(ChartOfAccountsData);

			var DoubleEntriesSetupData = ReadJsonFile<DoubleEntrySetup>(Path.Combine(dataDirectory, "DoubleEntriesSetupData.json"));
			modelBuilder.Entity<DoubleEntrySetup>().HasData(DoubleEntriesSetupData);

		}


		private static IEnumerable<T> ReadJsonFile<T>(string filePath)
		{
			var json = File.ReadAllText(filePath);
			return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
		}


		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var deletedInvoices = ChangeTracker.Entries<Invoice>()
				.Where(e => e.State == EntityState.Deleted)
				.ToList();

			foreach (var entry in deletedInvoices)
			{
				var invoiceId = entry.Entity.Id;

				var bankReceipt = await BankReceipts.FirstOrDefaultAsync(br => br.InvoiceId == invoiceId, cancellationToken);
				if (bankReceipt != null)
				{
					var doubleEntries = Accounting.Where(de => de.AccountingDocument == EAccountingDocument.BankReceipt && de.DocumentId == bankReceipt.Id);
					Accounting.RemoveRange(doubleEntries);
					BankReceipts.Remove(bankReceipt);
				}
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}
