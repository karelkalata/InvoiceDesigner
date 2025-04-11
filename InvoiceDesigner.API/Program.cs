using InvoiceDesigner.API.Helpers;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.Admin;
using InvoiceDesigner.Application.Interfaces.AdminInterfaces;
using InvoiceDesigner.Application.Interfaces.Documents;
using InvoiceDesigner.Application.Interfaces.InterfacesAccounting;
using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Application.Interfaces.Reports;
using InvoiceDesigner.Application.Services;
using InvoiceDesigner.Application.Services.Accounting;
using InvoiceDesigner.Application.Services.AdminService;
using InvoiceDesigner.Application.Services.Documents;
using InvoiceDesigner.Application.Services.Reports;
using InvoiceDesigner.Application.Services.ServiceFormDesigner;
using InvoiceDesigner.Application.Services.ServiceUser;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Domain.Shared.Interfaces.Accounting;
using InvoiceDesigner.Domain.Shared.Interfaces.Directories;
using InvoiceDesigner.Domain.Shared.Interfaces.Documents;
using InvoiceDesigner.Domain.Shared.Interfaces.Reports;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories;
using InvoiceDesigner.Infrastructure.Repositories.Accounting;
using InvoiceDesigner.Infrastructure.Repositories.Directories;
using InvoiceDesigner.Infrastructure.Repositories.Documents;
using InvoiceDesigner.Infrastructure.Repositories.Reports;
using InvoiceDesigner.Infrastructure.Serializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();

var jwtOption = builder.Configuration.GetSection("JWTOption");
string secretKey = jwtOption.GetValue<string>("SecretKey")
					?? throw new InvalidOperationException("SecretKey is null in appsettings.json");

builder.Services.AddAuthentication(option =>
{
	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
	{
		option.TokenValidationParameters = new()
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
		};
	});

builder.Services.AddAuthorization(option =>
{
	option.AddPolicy(UserPolicy.IsAdmin, policy => policy.RequireClaim(UserPolicy.IsAdmin, "true"));
});

#region Serializers dateTime format ISO 8601
builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
{
	options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((value, propertyName) =>
		$"The value '{value}' is not valid for {propertyName}.");
});

builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
	});
#endregion

//	AutoMapper is used solely to convert models to DTOs.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionSqLite")));
//  create migration
//      dotnet ef migrations add AddInitialMigration --startup-project InvoiceDesigner.API --project InvoiceDesigner.Infrastructure
//  create database
//      dotnet ef database update --project InvoiceDesigner.Infrastructure --startup-project InvoiceDesigner.Api

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ValidateUserIdFilter>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyServiceHelper, CompanyServiceHelper>();

builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IBankService, BankService>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductServiceHelper, ProductServiceHelper>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminUserInterface, AdminUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserServiceHelper, UserServiceHelper>();
builder.Services.AddScoped<IUserAuthorizedDataService, UserAuthorizedDataService>();
builder.Services.AddScoped<IAuthorizationUserService, AuthorizationUserService>();

#region FormDesigners
builder.Services.AddScoped<IFormDesignersRepository, FormDesignersRepository>();
builder.Services.AddScoped<IFormDesignersService, FormDesignersService>();
builder.Services.AddScoped<IDropItemsService, DropItemsService>();
builder.Services.AddScoped<ICssStyleService, CssStyleService>();
#endregion

builder.Services.AddScoped<IPrintInvoiceRepository, PrintInvoiceRepository>();
builder.Services.AddScoped<IPrintInvoiceService, PrintInvoiceService>();

#region Documents
builder.Services.AddScoped<IBankReceiptRepository, BankReceiptRepository>();
builder.Services.AddScoped<IBankReceiptService, BankReceiptService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceServiceHelper, InvoiceServiceHelper>();
#endregion

#region Real Accointing
builder.Services.AddScoped<IChartOfAccountsRepository, ChartOfAccountsRepository>();
builder.Services.AddScoped<IChartOfAccountsService, ChartOfAccountsService>();

builder.Services.AddScoped<IDoubleEntrySetupRepository, DoubleEntrySetupRepository>();
builder.Services.AddScoped<IDoubleEntrySetupService, DoubleEntrySetupService>();

builder.Services.AddScoped<IAccountingRepository, AccountingRepository>();
builder.Services.AddScoped<IAccountingService, AccountingService>();
#endregion

#region Reports Accointing
builder.Services.AddScoped<ITrialBalanceRepository, TrialBalanceRepository>();
builder.Services.AddScoped<ITrialBalanceService, TrialBalanceService>();

builder.Services.AddScoped<ICustomerDebitRepository, CustomerDebitRepository>();
builder.Services.AddScoped<ICustomerDebitService, CustomerDebitService>();
#endregion


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Logging.SetMinimumLevel(LogLevel.Information);

// Add HTTPS redirection and configure HSTS
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7048;
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseHsts();
app.MapDefaultEndpoints();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
