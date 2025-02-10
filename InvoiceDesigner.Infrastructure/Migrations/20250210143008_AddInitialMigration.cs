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
                    TypeChartOfAccount = table.Column<int>(type: "INTEGER", nullable: false),
                    Asset1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Asset2 = table.Column<int>(type: "INTEGER", nullable: false),
                    Asset3 = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
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
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    AccountingDocument = table.Column<int>(type: "INTEGER", nullable: false),
                    PageSizes = table.Column<int>(type: "INTEGER", nullable: false),
                    DynamicHeight = table.Column<bool>(type: "INTEGER", nullable: false),
                    PageMargin = table.Column<int>(type: "INTEGER", nullable: false),
                    PageOrientation = table.Column<int>(type: "INTEGER", nullable: false)
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
                    AmountType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsArchived = table.Column<bool>(type: "INTEGER", nullable: false)
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
                columns: new[] { "Id", "Asset1", "Asset2", "Asset3", "Code", "IsArchived", "IsDeleted", "Name", "TypeChartOfAccount" },
                values: new object[,]
                {
                    { 1, 2, 0, 0, 1200, false, false, "Accounts Receivable", 0 },
                    { 2, 1, 0, 0, 1030, false, false, "Bank: Operating", 0 },
                    { 3, 0, 0, 0, 2200, false, false, "Sales Tax", 1 },
                    { 4, 3, 0, 0, 4000, false, false, "Sales", 2 }
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
                columns: new[] { "Id", "AccountingDocument", "DynamicHeight", "Name", "PageMargin", "PageOrientation", "PageSizes" },
                values: new object[] { 1, 0, false, "default", 5, 0, 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsAdmin", "IsArchived", "IsDeleted", "Locale", "Login", "Name", "PasswordHash", "PasswordSalt" },
                values: new object[] { 1, true, false, false, "en-US", "admin", "Super Admin", "1708D30988E562DD2958B50B77F0D61C47C59FD7555F3B91AB02D486F361504F7E0C569157D104D99E5076BFF20AF9EE38482A63BA10993B28C38F9936668010", "7A6604F49A4E8EFCBB8B6CA86305FB0E4E14F817AE6D8726DE7A56463581A1D21D68699970298BBFE2182AE02366BCBB56C14DF47B9D000AF0C5D74DCED88953" });

            migrationBuilder.InsertData(
                table: "DoubleEntriesSetup",
                columns: new[] { "Id", "AccountingDocument", "AmountType", "Credit", "Debit", "EntryMode", "IsArchived", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, 0, 1, 4, 1, 0, false, false, "" },
                    { 2, 0, 2, 3, 1, 1, false, false, "" },
                    { 3, 1, 0, 1, 2, 1, false, false, "" }
                });

            migrationBuilder.InsertData(
                table: "DropItems",
                columns: new[] { "Id", "FormDesignerSchemeId", "Selector", "StartSelector", "UniqueId", "Value" },
                values: new object[,]
                {
                    { 401, 1, "coor_0_1", "_Invoice", "{Invoice.Number}", "INVOICE # {Invoice.Number}" },
                    { 402, 1, "coor_4_1", "_Invoice", "{Invoice.PONumber}", "P.O. #  {Invoice.PONumber}" },
                    { 403, 1, "coor_2_1", "_Invoice", "{Invoice.DateTime}", "DATE: {Invoice.DateTime}" },
                    { 404, 1, "coor_3_1", "_Invoice", "{Invoice.DueDate}", "DUE DATE: {Invoice.DueDate}" },
                    { 405, 1, "coor_0_0", "_Invoice", "{Invoice.Company}", "{Invoice.Company}" },
                    { 406, 1, "coor_11_0", "_Invoice", "{Invoice.InvoiceItems}", "{Invoice.InvoiceItems}" },
                    { 407, 1, "coor_2_0", "_Company", "{Company.TaxId}", "Tax Id: {Company.TaxId}" },
                    { 408, 1, "coor_3_0", "_Company", "{Company.VatId}", "Vat Id: {Company.VatId}" },
                    { 409, 1, "coor_5_1", "_Other", "7dae4134-bba4-49f1-bc47-d19a2f845ed6", "TO " },
                    { 410, 1, "coor_6_1", "_Customer", "{Customer.Name}", "{Customer.Name}" },
                    { 411, 1, "coor_7_1", "_Customer", "{Customer.TaxId}", "Tax Id: {Customer.TaxId}" },
                    { 412, 1, "coor_5_0", "_Bank", "{Bank.Name}", "{Bank.Name}" },
                    { 413, 1, "coor_6_0", "_Bank", "{Bank.BIC}", "{Bank.BIC}" },
                    { 414, 1, "coor_7_0", "_Bank", "{Bank.Account}", "{Bank.Account}" },
                    { 415, 1, "coor_13_0", "_Other", "5a0e0a8c-f374-4099-8295-9b3471af899c", "Karel Kalata" },
                    { 416, 1, "coor_9_0", "_Other", "25695f53-d1c5-46a0-b921-1a392fc1aba7", "If you have any questions, please contact us at {Company.Phone} or {Company.Email}" }
                });

            migrationBuilder.InsertData(
                table: "FormDesignerSchemes",
                columns: new[] { "Id", "Column", "FormDesignerId", "Row" },
                values: new object[,]
                {
                    { 1377, 1, 1, 0 },
                    { 1378, 1, 1, 1 },
                    { 1379, 1, 1, 2 },
                    { 1380, 1, 1, 3 },
                    { 1381, 1, 1, 4 },
                    { 1382, 1, 1, 5 },
                    { 1383, 1, 1, 6 },
                    { 1384, 1, 1, 7 },
                    { 1385, 0, 1, 8 },
                    { 1386, 0, 1, 9 },
                    { 1387, 0, 1, 10 },
                    { 1388, 0, 1, 11 },
                    { 1389, 0, 1, 12 },
                    { 1390, 1, 1, 13 },
                    { 1391, 0, 1, 14 },
                    { 1392, 0, 1, 15 },
                    { 1393, 0, 1, 16 },
                    { 1394, 0, 1, 17 },
                    { 1395, 0, 1, 18 },
                    { 1396, 0, 1, 19 },
                    { 1397, 0, 1, 20 },
                    { 1398, 0, 1, 21 },
                    { 1399, 0, 1, 22 },
                    { 1400, 0, 1, 23 },
                    { 1401, 0, 1, 24 },
                    { 1402, 0, 1, 25 },
                    { 1403, 0, 1, 26 },
                    { 1404, 0, 1, 27 },
                    { 1405, 0, 1, 28 },
                    { 1406, 0, 1, 29 },
                    { 1407, 0, 1, 30 },
                    { 1408, 0, 1, 31 }
                });

            migrationBuilder.InsertData(
                table: "DropItemStyles",
                columns: new[] { "Id", "DropItemId", "Name", "Value" },
                values: new object[,]
                {
                    { 2509, 401, "flex-grow", "1" },
                    { 2510, 401, "text-align", "right" },
                    { 2511, 401, "font-size", "16px" },
                    { 2512, 401, "height", "25px" },
                    { 2513, 401, "font-style", "normal" },
                    { 2514, 401, "font-weight", "normal" },
                    { 2515, 402, "flex-grow", "1" },
                    { 2516, 402, "text-align", "right" },
                    { 2517, 402, "font-size", "10px" },
                    { 2518, 402, "height", "25px" },
                    { 2519, 402, "font-style", "normal" },
                    { 2520, 402, "font-weight", "normal" },
                    { 2521, 403, "flex-grow", "1" },
                    { 2522, 403, "text-align", "right" },
                    { 2523, 403, "font-size", "10px" },
                    { 2524, 403, "height", "25px" },
                    { 2525, 403, "font-style", "normal" },
                    { 2526, 403, "font-weight", "normal" },
                    { 2527, 404, "flex-grow", "1" },
                    { 2528, 404, "text-align", "right" },
                    { 2529, 404, "font-size", "10px" },
                    { 2530, 404, "height", "25px" },
                    { 2531, 404, "font-style", "normal" },
                    { 2532, 404, "font-weight", "normal" },
                    { 2533, 405, "flex-grow", "1" },
                    { 2534, 405, "text-align", "left" },
                    { 2535, 405, "font-size", "16px" },
                    { 2536, 405, "height", "25px" },
                    { 2537, 405, "font-style", "normal" },
                    { 2538, 405, "font-weight", "normal" },
                    { 2539, 406, "flex-grow", "1" },
                    { 2540, 406, "text-align", "left" },
                    { 2541, 406, "font-size", "10px" },
                    { 2542, 406, "height", "25px" },
                    { 2543, 406, "font-style", "normal" },
                    { 2544, 406, "font-weight", "normal" },
                    { 2545, 406, "AddCurrencySymbol", "none" },
                    { 2546, 406, "AddCurrencySymbolFooter", "right" },
                    { 2547, 406, "FooterLeftMargin", "75 %" },
                    { 2548, 407, "flex-grow", "1" },
                    { 2549, 407, "text-align", "left" },
                    { 2550, 407, "font-size", "10px" },
                    { 2551, 407, "height", "25px" },
                    { 2552, 407, "font-style", "normal" },
                    { 2553, 407, "font-weight", "normal" },
                    { 2554, 408, "flex-grow", "1" },
                    { 2555, 408, "text-align", "left" },
                    { 2556, 408, "font-size", "10px" },
                    { 2557, 408, "height", "25px" },
                    { 2558, 408, "font-style", "normal" },
                    { 2559, 408, "font-weight", "normal" },
                    { 2560, 409, "flex-grow", "1" },
                    { 2561, 409, "text-align", "left" },
                    { 2562, 409, "font-size", "10px" },
                    { 2563, 409, "height", "25px" },
                    { 2564, 409, "font-style", "normal" },
                    { 2565, 409, "font-weight", "bold" },
                    { 2566, 410, "flex-grow", "1" },
                    { 2567, 410, "text-align", "left" },
                    { 2568, 410, "font-size", "10px" },
                    { 2569, 410, "height", "25px" },
                    { 2570, 410, "font-style", "normal" },
                    { 2571, 410, "font-weight", "normal" },
                    { 2572, 411, "flex-grow", "1" },
                    { 2573, 411, "text-align", "left" },
                    { 2574, 411, "font-size", "10px" },
                    { 2575, 411, "height", "25px" },
                    { 2576, 411, "font-style", "normal" },
                    { 2577, 411, "font-weight", "normal" },
                    { 2578, 412, "flex-grow", "1" },
                    { 2579, 412, "text-align", "left" },
                    { 2580, 412, "font-size", "10px" },
                    { 2581, 412, "height", "25px" },
                    { 2582, 412, "font-style", "normal" },
                    { 2583, 412, "font-weight", "normal" },
                    { 2584, 413, "flex-grow", "1" },
                    { 2585, 413, "text-align", "left" },
                    { 2586, 413, "font-size", "10px" },
                    { 2587, 413, "height", "25px" },
                    { 2588, 413, "font-style", "normal" },
                    { 2589, 413, "font-weight", "normal" },
                    { 2590, 414, "flex-grow", "1" },
                    { 2591, 414, "text-align", "left" },
                    { 2592, 414, "font-size", "10px" },
                    { 2593, 414, "height", "25px" },
                    { 2594, 414, "font-style", "normal" },
                    { 2595, 414, "font-weight", "normal" },
                    { 2596, 415, "flex-grow", "1" },
                    { 2597, 415, "text-align", "center" },
                    { 2598, 415, "font-size", "12px" },
                    { 2599, 415, "height", "25px" },
                    { 2600, 415, "font-style", "italic" },
                    { 2601, 415, "font-weight", "normal" },
                    { 2602, 416, "flex-grow", "1" },
                    { 2603, 416, "text-align", "left" },
                    { 2604, 416, "font-size", "8px" },
                    { 2605, 416, "height", "25px" },
                    { 2606, 416, "font-style", "italic" },
                    { 2607, 416, "font-weight", "normal" }
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
