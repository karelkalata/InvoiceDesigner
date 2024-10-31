using InvoiceDesigner.Domain.Shared.Models;
using InvoiceDesigner.Domain.Shared.Models.FormDesigner;
using Microsoft.EntityFrameworkCore;

namespace InvoiceDesigner.Infrastructure.Data
{
	public class DataContext : DbContext
	{

		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<Company> Companies { get; set; }
		public DbSet<Currency> Currencies { get; set; }
		public DbSet<Bank> Banks { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductPrice> ProductPrice { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<InvoiceItem> InvoiceItems { get; set; }

		public DbSet<FormDesigner> FormDesigners { get; set; }
		public DbSet<DropItem> DropItems { get; set; }
		public DbSet<DropItemCssStyle> DropItemStyles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

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
					.WithMany()
					.HasForeignKey(e => e.CompanyId)
					.IsRequired();
			});

			modelBuilder.Entity<Company>(company =>
			{
				company.HasKey(e => e.Id);

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

				invoice.HasOne(e => e.Bank)
					.WithMany()
					.HasForeignKey(e => e.BankId)
					.IsRequired();

				invoice.Property(e => e.TotalAmount)
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

				invoiceItem.HasOne(e => e.Product)
					.WithMany()
					.HasForeignKey(e => e.ProductId)
					.IsRequired();

				invoiceItem.Property(e => e.Price)
					.IsRequired()
					.HasDefaultValue(decimal.Zero);

				invoiceItem.Property(e => e.Quantity)
					.IsRequired()
					.HasDefaultValue(decimal.Zero);
			});


			modelBuilder.Entity<FormDesigner>(formDesigner =>
			{
				formDesigner.HasKey(e => e.Id);

				formDesigner.Property(e => e.Name)
					.IsRequired();

				formDesigner.Property(e => e.Rows)
					.HasDefaultValue(32);

				formDesigner.Property(e => e.Columns)
					.HasDefaultValue(3);

				formDesigner.HasMany(e => e.DropItems)
					.WithOne(e => e.FormDesigner)
					.HasForeignKey(e => e.FormDesignerId)
					.OnDelete(DeleteBehavior.Cascade)
					.IsRequired();
			});

			modelBuilder.Entity<DropItem>(dropItem =>
			{
				dropItem.HasKey(e => e.Id);

				dropItem.Property(e => e.UniqueId)
					.IsRequired();

				dropItem.Property(e => e.Name)
					.IsRequired();

				dropItem.Property(e => e.Selector)
					.IsRequired();

				dropItem.Property(e => e.StartSelector)
					.IsRequired();

				dropItem.HasOne(e => e.FormDesigner)
					.WithMany()
					.HasForeignKey(e => e.FormDesignerId)
					.OnDelete(DeleteBehavior.Cascade);

				dropItem.HasMany(e => e.CssStyle)
					.WithOne(e => e.DropItem)
					.HasForeignKey(e => e.DropItemId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<DropItemCssStyle>(cssStyle =>
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

		}
	}
}
