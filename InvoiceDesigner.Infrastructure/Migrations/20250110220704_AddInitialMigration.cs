using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InvoiceDesigner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChartOfAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Asset1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Asset2 = table.Column<int>(type: "INTEGER", nullable: false),
                    Asset3 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartOfAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    VatId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormDesigners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountingDocument = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDesigners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintInvoices",
                columns: table => new
                {
                    Giud = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrintFormId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintInvoices", x => x.Giud);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Locale = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordSalt = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoubleEntriesSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountingDocument = table.Column<int>(type: "INTEGER", nullable: false),
                    Credit = table.Column<int>(type: "INTEGER", nullable: false),
                    Debit = table.Column<int>(type: "INTEGER", nullable: false),
                    EntryMode = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleEntriesSetup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoubleEntriesSetup_ChartOfAccounts_Credit",
                        column: x => x.Credit,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoubleEntriesSetup_ChartOfAccounts_Debit",
                        column: x => x.Debit,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VatId = table.Column<string>(type: "TEXT", nullable: true),
                    WWW = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Info = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentTerms = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultVat = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DropItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UniqueId = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Selector = table.Column<string>(type: "TEXT", nullable: false),
                    StartSelector = table.Column<string>(type: "TEXT", nullable: false),
                    FormDesignerSchemeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropItems_FormDesigners_FormDesignerSchemeId",
                        column: x => x.FormDesignerSchemeId,
                        principalTable: "FormDesigners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormDesignerSchemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Row = table.Column<int>(type: "INTEGER", nullable: false),
                    Column = table.Column<int>(type: "INTEGER", nullable: false),
                    FormDesignerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDesignerSchemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormDesignerSchemes_FormDesigners_FormDesignerId",
                        column: x => x.FormDesignerId,
                        principalTable: "FormDesigners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrice_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPrice_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountingDocument = table.Column<int>(type: "INTEGER", nullable: false),
                    DocumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Debit = table.Column<int>(type: "INTEGER", nullable: false),
                    DebitAsset1 = table.Column<int>(type: "INTEGER", nullable: false),
                    DebitAsset2 = table.Column<int>(type: "INTEGER", nullable: false),
                    DebitAsset3 = table.Column<int>(type: "INTEGER", nullable: false),
                    Credit = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditAsset1 = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditAsset2 = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditAsset3 = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<decimal>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounting_ChartOfAccounts_Credit",
                        column: x => x.Credit,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounting_ChartOfAccounts_Debit",
                        column: x => x.Debit,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounting_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounting_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BIC = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    Account = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Banks_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCompany",
                columns: table => new
                {
                    ActivityUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompany", x => new { x.ActivityUserId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_UserCompany_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCompany_Users_ActivityUserId",
                        column: x => x.ActivityUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DropItemStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    DropItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropItemStyles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropItemStyles_DropItems_DropItemId",
                        column: x => x.DropItemId,
                        principalTable: "DropItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PONumber = table.Column<string>(type: "TEXT", nullable: false),
                    Vat = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m),
                    EnabledVat = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    BankId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    BankId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankReceipts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankReceipts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankReceipts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankReceipts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankReceipts_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ChartOfAccounts",
                columns: new[] { "Id", "Asset1", "Asset2", "Asset3", "Code", "Name" },
                values: new object[,]
                {
                    { 1, 2, 0, 0, 1200, "Accounts Receivable" },
                    { 2, 1, 0, 0, 1030, "Bank: Operating" },
                    { 3, 0, 0, 0, 2200, "Sales Tax" },
                    { 4, 3, 0, 0, 4000, "Sales" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Description", "IsArchived", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "US Dollar", false, false, "USD" },
                    { 2, "Euro", false, false, "EUR" },
                    { 3, "Czech Koruna", false, false, "CZK" }
                });

            migrationBuilder.InsertData(
                table: "FormDesigners",
                columns: new[] { "Id", "AccountingDocument", "Name" },
                values: new object[] { 1, 0, "default" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsAdmin", "IsArchived", "IsDeleted", "Locale", "Login", "Name", "PasswordHash", "PasswordSalt" },
                values: new object[] { 1, true, false, false, "en-US", "admin", "Super Admin", "1708D30988E562DD2958B50B77F0D61C47C59FD7555F3B91AB02D486F361504F7E0C569157D104D99E5076BFF20AF9EE38482A63BA10993B28C38F9936668010", "7A6604F49A4E8EFCBB8B6CA86305FB0E4E14F817AE6D8726DE7A56463581A1D21D68699970298BBFE2182AE02366BCBB56C14DF47B9D000AF0C5D74DCED88953" });

            migrationBuilder.InsertData(
                table: "DoubleEntriesSetup",
                columns: new[] { "Id", "AccountingDocument", "AmountType", "Credit", "Debit", "EntryMode" },
                values: new object[,]
                {
                    { 1, 0, 1, 4, 1, 0 },
                    { 2, 0, 2, 3, 1, 1 },
                    { 3, 1, 1, 1, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "DropItems",
                columns: new[] { "Id", "FormDesignerSchemeId", "Selector", "StartSelector", "UniqueId", "Value" },
                values: new object[,]
                {
                    { 967, 1, "coor_0_0", "_Invoice", "{Invoice.Number}", "Invoice #{Invoice.Number}" },
                    { 968, 1, "coor_1_2", "_Invoice", "{Invoice.DateTime}", "Issue Date: {Invoice.DateTime}" },
                    { 969, 1, "coor_2_2", "_Invoice", "{Invoice.DueDate}", "Due Date: {Invoice.DueDate}" },
                    { 970, 1, "coor_3_2", "_Invoice", "{Invoice.PONumber}", "PO: {Invoice.PONumber}" },
                    { 971, 1, "coor_11_0", "_Invoice", "{Invoice.InvoiceItems}", "{Invoice.InvoiceItems}" },
                    { 972, 1, "coor_5_2", "_Customer", "{Customer.Name}", "{Customer.Name}" },
                    { 973, 1, "coor_6_2", "_Customer", "{Customer.TaxId}", "Tax ID: {Customer.TaxId}" },
                    { 974, 1, "coor_2_0", "_Company", "{Company.Name}", "{Company.Name}" },
                    { 975, 1, "coor_3_0", "_Company", "{Company.TaxId}", "Tax ID: {Company.TaxId}" },
                    { 976, 1, "coor_4_0", "_Company", "{Company.VatId}", "Vat ID: {Company.VatId}" },
                    { 977, 1, "coor_4_2", "_Other", "c09e7e7f-1700-4eb7-b83e-4e9b0be08e66", "Bill  To:" },
                    { 978, 1, "coor_15_0", "_Other", "ebd7a5ce-37fc-4d21-9651-38390fb4fd69", "Karel Kalata" },
                    { 979, 1, "coor_5_0", "_Bank", "{Bank.Name}", "Bank: {Bank.Name}" },
                    { 980, 1, "coor_6_0", "_Bank", "{Bank.BIC}", "BIC/SWIFT: {Bank.BIC}" },
                    { 981, 1, "coor_7_0", "_Bank", "{Bank.Account}", "{Bank.Account}" },
                    { 982, 1, "coor_9_0", "_Other", "ec235007-1e77-4be7-9e87-c7552fb9776a", "If you have any questions, please contact us at {Company.Phone} or {Company.Email}" }
                });

            migrationBuilder.InsertData(
                table: "FormDesignerSchemes",
                columns: new[] { "Id", "Column", "FormDesignerId", "Row" },
                values: new object[,]
                {
                    { 2465, 0, 1, 0 },
                    { 2466, 2, 1, 1 },
                    { 2467, 2, 1, 2 },
                    { 2468, 2, 1, 3 },
                    { 2469, 2, 1, 4 },
                    { 2470, 2, 1, 5 },
                    { 2471, 2, 1, 6 },
                    { 2472, 2, 1, 7 },
                    { 2473, 0, 1, 8 },
                    { 2474, 0, 1, 9 },
                    { 2475, 0, 1, 10 },
                    { 2476, 0, 1, 11 },
                    { 2477, 0, 1, 12 },
                    { 2478, 0, 1, 13 },
                    { 2479, 0, 1, 14 },
                    { 2480, 1, 1, 15 },
                    { 2481, 0, 1, 16 },
                    { 2482, 0, 1, 17 },
                    { 2483, 0, 1, 18 },
                    { 2484, 0, 1, 19 },
                    { 2485, 0, 1, 20 },
                    { 2486, 0, 1, 21 },
                    { 2487, 0, 1, 22 },
                    { 2488, 0, 1, 23 },
                    { 2489, 0, 1, 24 },
                    { 2490, 0, 1, 25 },
                    { 2491, 0, 1, 26 },
                    { 2492, 0, 1, 27 },
                    { 2493, 0, 1, 28 },
                    { 2494, 0, 1, 29 },
                    { 2495, 0, 1, 30 },
                    { 2496, 0, 1, 31 }
                });

            migrationBuilder.InsertData(
                table: "DropItemStyles",
                columns: new[] { "Id", "DropItemId", "Name", "Value" },
                values: new object[,]
                {
                    { 5925, 967, "flex-grow", "1" },
                    { 5926, 967, "text-align", "center" },
                    { 5927, 967, "font-size", "16px" },
                    { 5928, 967, "height", "25px" },
                    { 5929, 967, "font-style", "normal" },
                    { 5930, 967, "font-weight", "bold" },
                    { 5931, 968, "flex-grow", "1" },
                    { 5932, 968, "text-align", "right" },
                    { 5933, 968, "font-size", "10px" },
                    { 5934, 968, "height", "25px" },
                    { 5935, 968, "font-style", "normal" },
                    { 5936, 968, "font-weight", "normal" },
                    { 5937, 969, "flex-grow", "1" },
                    { 5938, 969, "text-align", "right" },
                    { 5939, 969, "font-size", "10px" },
                    { 5940, 969, "height", "25px" },
                    { 5941, 969, "font-style", "normal" },
                    { 5942, 969, "font-weight", "normal" },
                    { 5943, 970, "flex-grow", "1" },
                    { 5944, 970, "text-align", "right" },
                    { 5945, 970, "font-size", "10px" },
                    { 5946, 970, "height", "25px" },
                    { 5947, 970, "font-style", "normal" },
                    { 5948, 970, "font-weight", "normal" },
                    { 5949, 971, "flex-grow", "1" },
                    { 5950, 971, "text-align", "left" },
                    { 5951, 971, "font-size", "10px" },
                    { 5952, 971, "height", "25px" },
                    { 5953, 971, "font-style", "italic" },
                    { 5954, 971, "font-weight", "normal" },
                    { 5955, 971, "AddCurrencySymbol", "left" },
                    { 5956, 971, "AddCurrencySymbolFooter", "right" },
                    { 5957, 972, "flex-grow", "1" },
                    { 5958, 972, "text-align", "left" },
                    { 5959, 972, "font-size", "12px" },
                    { 5960, 972, "height", "25px" },
                    { 5961, 972, "font-style", "normal" },
                    { 5962, 972, "font-weight", "normal" },
                    { 5963, 973, "flex-grow", "1" },
                    { 5964, 973, "text-align", "left" },
                    { 5965, 973, "font-size", "10px" },
                    { 5966, 973, "height", "25px" },
                    { 5967, 973, "font-style", "normal" },
                    { 5968, 973, "font-weight", "normal" },
                    { 5969, 974, "flex-grow", "1" },
                    { 5970, 974, "text-align", "left" },
                    { 5971, 974, "font-size", "12px" },
                    { 5972, 974, "height", "25px" },
                    { 5973, 974, "font-style", "normal" },
                    { 5974, 974, "font-weight", "bold" },
                    { 5975, 975, "flex-grow", "1" },
                    { 5976, 975, "text-align", "left" },
                    { 5977, 975, "font-size", "10px" },
                    { 5978, 975, "height", "25px" },
                    { 5979, 975, "font-style", "normal" },
                    { 5980, 975, "font-weight", "normal" },
                    { 5981, 976, "flex-grow", "1" },
                    { 5982, 976, "text-align", "left" },
                    { 5983, 976, "font-size", "10px" },
                    { 5984, 976, "height", "25px" },
                    { 5985, 976, "font-style", "normal" },
                    { 5986, 976, "font-weight", "normal" },
                    { 5987, 977, "flex-grow", "1" },
                    { 5988, 977, "text-align", "left" },
                    { 5989, 977, "font-size", "10px" },
                    { 5990, 977, "height", "25px" },
                    { 5991, 977, "font-style", "normal" },
                    { 5992, 977, "font-weight", "bold" },
                    { 5993, 978, "flex-grow", "1" },
                    { 5994, 978, "text-align", "center" },
                    { 5995, 978, "font-size", "10px" },
                    { 5996, 978, "height", "25px" },
                    { 5997, 978, "font-style", "italic" },
                    { 5998, 978, "font-weight", "normal" },
                    { 5999, 979, "flex-grow", "1" },
                    { 6000, 979, "text-align", "left" },
                    { 6001, 979, "font-size", "12px" },
                    { 6002, 979, "height", "25px" },
                    { 6003, 979, "font-style", "normal" },
                    { 6004, 979, "font-weight", "normal" },
                    { 6005, 980, "flex-grow", "1" },
                    { 6006, 980, "text-align", "left" },
                    { 6007, 980, "font-size", "12px" },
                    { 6008, 980, "height", "25px" },
                    { 6009, 980, "font-style", "normal" },
                    { 6010, 980, "font-weight", "bold" },
                    { 6011, 981, "flex-grow", "1" },
                    { 6012, 981, "text-align", "left" },
                    { 6013, 981, "font-size", "12px" },
                    { 6014, 981, "height", "25px" },
                    { 6015, 981, "font-style", "normal" },
                    { 6016, 981, "font-weight", "bold" },
                    { 6017, 982, "flex-grow", "1" },
                    { 6018, 982, "text-align", "left" },
                    { 6019, 982, "font-size", "10px" },
                    { 6020, 982, "height", "25px" },
                    { 6021, 982, "font-style", "italic" },
                    { 6022, 982, "font-weight", "normal" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounting_CompanyId",
                table: "Accounting",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounting_Credit",
                table: "Accounting",
                column: "Credit");

            migrationBuilder.CreateIndex(
                name: "IX_Accounting_CurrencyId",
                table: "Accounting",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounting_Debit",
                table: "Accounting",
                column: "Debit");

            migrationBuilder.CreateIndex(
                name: "IX_BankReceipts_BankId",
                table: "BankReceipts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankReceipts_CompanyId",
                table: "BankReceipts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankReceipts_CurrencyId",
                table: "BankReceipts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankReceipts_CustomerId",
                table: "BankReceipts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BankReceipts_InvoiceId",
                table: "BankReceipts",
                column: "InvoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CompanyId",
                table: "Banks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CurrencyId",
                table: "Banks",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrencyId",
                table: "Companies",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoubleEntriesSetup_Credit",
                table: "DoubleEntriesSetup",
                column: "Credit");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleEntriesSetup_Debit",
                table: "DoubleEntriesSetup",
                column: "Debit");

            migrationBuilder.CreateIndex(
                name: "IX_DropItems_FormDesignerSchemeId",
                table: "DropItems",
                column: "FormDesignerSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_DropItemStyles_DropItemId",
                table: "DropItemStyles",
                column: "DropItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDesignerSchemes_FormDesignerId",
                table: "FormDesignerSchemes",
                column: "FormDesignerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ItemId",
                table: "InvoiceItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BankId",
                table: "Invoices",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CompanyId",
                table: "Invoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CurrencyId",
                table: "Invoices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_CurrencyId",
                table: "ProductPrice",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrice_ProductId",
                table: "ProductPrice",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompany_CompanyId",
                table: "UserCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounting");

            migrationBuilder.DropTable(
                name: "BankReceipts");

            migrationBuilder.DropTable(
                name: "DoubleEntriesSetup");

            migrationBuilder.DropTable(
                name: "DropItemStyles");

            migrationBuilder.DropTable(
                name: "FormDesignerSchemes");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "PrintInvoices");

            migrationBuilder.DropTable(
                name: "ProductPrice");

            migrationBuilder.DropTable(
                name: "UserCompany");

            migrationBuilder.DropTable(
                name: "ChartOfAccounts");

            migrationBuilder.DropTable(
                name: "DropItems");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FormDesigners");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
