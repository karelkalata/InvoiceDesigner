using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Models.Directories;
using InvoiceDesigner.Domain.Shared.Models.Documents;
using InvoiceDesigner.Domain.Shared.Models.ModelsAccounting;
using InvoiceDesigner.Domain.Shared.Models.ModelsFormDesigner;
using Microsoft.EntityFrameworkCore;
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

			modelBuilder.Entity<Currency>(currency =>
			{
				currency.HasKey(e => e.Id);

				currency.HasIndex(e => e.Name).IsUnique();

				currency.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(3);

				currency.Property(e => e.Description)
					.HasMaxLength(100);

				currency.HasData(
					new Currency { Id = 1, Name = "USD", Description = "US Dollar" },
					new Currency { Id = 2, Name = "EUR", Description = "Euro" },
					new Currency { Id = 3, Name = "CZK", Description = "Czech Koruna" }
				);
			});

			modelBuilder.Entity<Bank>(bank =>
			{
				bank.HasKey(e => e.Id);

				bank.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(200);

				bank.Property(e => e.BIC)
					.HasMaxLength(11);

				bank.Property(e => e.Account)
					.IsRequired()
					.HasMaxLength(50);

				bank.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.IsRequired();

				bank.HasOne(e => e.Company)
					.WithMany(c => c.Banks)
					.HasForeignKey(e => e.CompanyId)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Company>(company =>
			{
				company.HasKey(e => e.Id);

				company.HasIndex(e => e.Name).IsUnique();

				company.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(300);

				company.Property(e => e.TaxId)
					.IsRequired()
					.HasMaxLength(50);

				company.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.IsRequired();
			});

			modelBuilder.Entity<Customer>(customer =>
			{
				customer.HasKey(e => e.Id);

				customer.Property(e => e.Name)
					.IsRequired();

				customer.Property(e => e.TaxId)
					.HasMaxLength(50);

				customer.Property(e => e.VatId)
					.HasMaxLength(50);
			});

			modelBuilder.Entity<Product>(product =>
			{
				product.HasKey(e => e.Id);

				product.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(200);

				product.HasMany(e => e.ProductPrice)
					.WithOne()
					.HasForeignKey(e => e.ProductId)
					.OnDelete(DeleteBehavior.Cascade);

			});

			modelBuilder.Entity<ProductPrice>(productPrice =>
			{
				productPrice.HasKey(e => e.Id);

				productPrice.Property(e => e.Price)
					.IsRequired();

				productPrice.HasOne<Product>()
					.WithMany(e => e.ProductPrice)
					.HasForeignKey(e => e.ProductId)
					.OnDelete(DeleteBehavior.Cascade);

				productPrice.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<User>(user =>
			{
				user.HasKey(e => e.Id);

				user.HasIndex(e => e.Login).IsUnique();

				user.Property(e => e.Login)
					.IsRequired()
					.HasMaxLength(100);

				user.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(100);

				user.Property(e => e.PasswordHash)
					.IsRequired();

				user.Property(e => e.PasswordSalt)
					.IsRequired();

				user.HasMany(e => e.Companies)
					.WithMany()
					.UsingEntity<Dictionary<string, object>>(
						"UserCompany",
						uc => uc.HasOne<Company>().WithMany().HasForeignKey("CompanyId").OnDelete(DeleteBehavior.Cascade),
						uc => uc.HasOne<User>().WithMany().HasForeignKey("ActivityUserId").OnDelete(DeleteBehavior.Cascade)
					);

				// default login: admin, password: admin
				user.HasData(
						new User
						{
							Id = 1,
							Login = "admin",
							Name = "Super Admin",
							PasswordHash = "1708D30988E562DD2958B50B77F0D61C47C59FD7555F3B91AB02D486F361504F7E0C569157D104D99E5076BFF20AF9EE38482A63BA10993B28C38F9936668010",
							PasswordSalt = "7A6604F49A4E8EFCBB8B6CA86305FB0E4E14F817AE6D8726DE7A56463581A1D21D68699970298BBFE2182AE02366BCBB56C14DF47B9D000AF0C5D74DCED88953",
							IsAdmin = true
						}
					);
			});

			modelBuilder.Entity<PrintInvoice>(PrintInvoice =>
			{
				PrintInvoice.HasKey(e => e.Giud);

			});

			#region Documents
			modelBuilder.Entity<BankReceipt>(bankReceipt =>
			{
				bankReceipt.HasKey(e => e.Id);


				bankReceipt.HasOne(e => e.Invoice)
					//.WithOne(invoice => invoice.BankReceipt)
					.WithOne()
					.HasForeignKey<BankReceipt>(e => e.InvoiceId)
					.IsRequired();


				bankReceipt.HasOne(e => e.Company)
					.WithMany()
					.HasForeignKey(e => e.CompanyId)
					.IsRequired();

				bankReceipt.HasOne(e => e.Customer)
					.WithMany()
					.HasForeignKey(e => e.CustomerId)
					.IsRequired();

				bankReceipt.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.IsRequired();

				bankReceipt.HasOne(e => e.Bank)
					.WithMany()
					.HasForeignKey(e => e.BankId)
					.IsRequired();

				bankReceipt.Property(e => e.Amount)
					.HasDefaultValue(decimal.Zero);

			});

			modelBuilder.Entity<Invoice>(invoice =>
			{
				invoice.HasKey(e => e.Id);

				invoice.HasOne(e => e.Company)
					.WithMany()
					.HasForeignKey(e => e.CompanyId)
					.IsRequired();

				invoice.Property(e => e.Vat)
					.HasDefaultValue(decimal.Zero);

				invoice.Property(e => e.EnabledVat)
					.HasDefaultValue(true);

				invoice.HasOne(e => e.Customer)
					.WithMany()
					.HasForeignKey(e => e.CustomerId)
					.IsRequired();

				invoice.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.IsRequired();
				/*
				invoice.HasOne(e => e.BankReceipt)
					.WithOne(bankReceipt => bankReceipt.InvoiceDTOs)
					.HasForeignKey<BankReceipt>(e => e.InvoiceId)
					.IsRequired(false);
				*/

				invoice.HasOne(e => e.Bank)
					.WithMany()
					.HasForeignKey(e => e.BankId)
					.IsRequired();

				invoice.Property(e => e.Amount)
					.HasDefaultValue(decimal.Zero);

				invoice.HasMany(e => e.InvoiceItems)
					.WithOne()
					.IsRequired();
			});

			modelBuilder.Entity<InvoiceItem>(invoiceItem =>
			{
				invoiceItem.HasKey(e => e.Id);

				invoiceItem.HasOne(e => e.Invoice)
					.WithMany(e => e.InvoiceItems)
					.HasForeignKey(e => e.InvoiceId)
					.IsRequired();

				invoiceItem.HasOne(e => e.Item)
					.WithMany()
					.HasForeignKey(e => e.ItemId)
					.IsRequired();

				invoiceItem.Property(e => e.Price)
					.IsRequired()
					.HasDefaultValue(decimal.Zero);

				invoiceItem.Property(e => e.Quantity)
					.IsRequired()
					.HasDefaultValue(decimal.Zero);
			});
			#endregion

			#region FormDesigner
			modelBuilder.Entity<FormDesigner>(formDesigner =>
			{
				formDesigner.HasKey(e => e.Id);

				formDesigner.Property(e => e.Name)
					.IsRequired();

				formDesigner.HasMany(e => e.Schemes)
					.WithOne()
					.HasForeignKey(e => e.FormDesignerId)
					.OnDelete(DeleteBehavior.Cascade);

				formDesigner.HasMany(e => e.DropItems)
					.WithOne()
					.HasForeignKey(e => e.FormDesignerSchemeId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<FormDesignerScheme>(formDesignerScheme =>
			{
				formDesignerScheme.HasKey(e => e.Id);

			});

			modelBuilder.Entity<DropItem>(dropItem =>
			{
				dropItem.HasKey(e => e.Id);

				dropItem.Property(e => e.UniqueId)
					.IsRequired();

				dropItem.Property(e => e.Selector)
					.IsRequired();

				dropItem.Property(e => e.StartSelector)
					.IsRequired();

				dropItem.HasMany(e => e.CssStyle)
					.WithOne(e => e.DropItem)
					.HasForeignKey(e => e.DropItemId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<CssStyle>(cssStyle =>
			{
				cssStyle.HasKey(e => e.Id);

				cssStyle.Property(e => e.Name)
					.IsRequired();

				cssStyle.Property(e => e.Value)
					.IsRequired();

				cssStyle.HasOne(e => e.DropItem)
					.WithMany(e => e.CssStyle)
					.HasForeignKey(e => e.DropItemId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			var FormDesignerEntities = ReadJsonFile<FormDesigner>(Path.Combine(dataDirectory, "FormDesigners.json"));
			modelBuilder.Entity<FormDesigner>().HasData(FormDesignerEntities);

			var FormDesignerSchemeEntities = ReadJsonFile<FormDesignerScheme>(Path.Combine(dataDirectory, "FormDesignerSchemes.json"));
			modelBuilder.Entity<FormDesignerScheme>().HasData(FormDesignerSchemeEntities);

			var DropItemEntities = ReadJsonFile<DropItem>(Path.Combine(dataDirectory, "DropItems.json"));
			modelBuilder.Entity<DropItem>().HasData(DropItemEntities);

			var DropItemStylesEntities = ReadJsonFile<CssStyle>(Path.Combine(dataDirectory, "DropItemStyles.json"));
			modelBuilder.Entity<CssStyle>().HasData(DropItemStylesEntities);

			#endregion

			#region Real Accounting
			modelBuilder.Entity<ChartOfAccounts>(chartOfAccounts =>
			{
				chartOfAccounts.HasKey(e => e.Id);

				chartOfAccounts.Property(e => e.Code).IsRequired();
				chartOfAccounts.Property(e => e.Name).IsRequired();

				var ChartOfAccountsData = ReadJsonFile<ChartOfAccounts>(Path.Combine(dataDirectory, "ChartOfAccountsData.json"));
				modelBuilder.Entity<ChartOfAccounts>().HasData(ChartOfAccountsData);

			});

			modelBuilder.Entity<DoubleEntrySetup>(doubleEntrySetup =>
			{
				doubleEntrySetup.HasKey(e => e.Id);

				doubleEntrySetup.HasOne(e => e.CreditAccount)
					.WithMany()
					.HasForeignKey(e => e.Credit)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

				doubleEntrySetup.HasOne(e => e.DebitAccount)
					.WithMany()
					.HasForeignKey(e => e.Debit)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

				doubleEntrySetup.Property(e => e.EntryMode).IsRequired();
				doubleEntrySetup.Property(e => e.AmountType).IsRequired();

				var DoubleEntriesSetupData = ReadJsonFile<DoubleEntrySetup>(Path.Combine(dataDirectory, "DoubleEntriesSetupData.json"));
				modelBuilder.Entity<DoubleEntrySetup>().HasData(DoubleEntriesSetupData);
			});

			modelBuilder.Entity<DoubleEntry>(doubleEntry =>
			{
				doubleEntry.HasKey(e => e.Id);

				doubleEntry.HasOne(e => e.CreditAccount)
					.WithMany()
					.HasForeignKey(e => e.Credit)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

				doubleEntry.HasOne(e => e.DebitAccount)
					.WithMany()
					.HasForeignKey(e => e.Debit)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

				doubleEntry.HasOne(e => e.Company)
					.WithMany()
					.HasForeignKey(e => e.CompanyId)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

				doubleEntry.HasOne(e => e.Currency)
					.WithMany()
					.HasForeignKey(e => e.CurrencyId)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired();

				doubleEntry.Property(e => e.Amount).IsRequired();
			});

			#endregion

		}


		private static IEnumerable<T> ReadJsonFile<T>(string filePath)
		{
			var json = File.ReadAllText(filePath);
			return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
		}
	}
}
