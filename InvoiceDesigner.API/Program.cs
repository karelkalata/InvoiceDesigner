using InvoiceDesigner.API.Helpers;
using InvoiceDesigner.Application.Authorization;
using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Interfaces.AdminInterfaces;
using InvoiceDesigner.Application.Interfaces.InterfacesFormDesigner;
using InvoiceDesigner.Application.Interfaces.InterfacesUser;
using InvoiceDesigner.Application.Services;
using InvoiceDesigner.Application.Services.AdminService;
using InvoiceDesigner.Application.Services.ServiceFormDesigner;
using InvoiceDesigner.Application.Services.ServiceUser;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddScoped<IBankServiceHelper, BankServiceHelper>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductServiceHelper, ProductServiceHelper>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceServiceHelper, InvoiceServiceHelper>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminUserInterface, AdminUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserServiceHelper, UserServiceHelper>();
builder.Services.AddScoped<IUserAuthorizedDataService, UserAuthorizedDataService>();
builder.Services.AddScoped<IAuthorizationUserService, AuthorizationUserService>();

builder.Services.AddScoped<IUserActivityLogRepository, UserActivityLogRepository>();
builder.Services.AddScoped<IUserActivityLogService, UserActivityLogService>();

builder.Services.AddScoped<IFormDesignersRepository, FormDesignersRepository>();
builder.Services.AddScoped<IFormDesignersService, FormDesignersService>();

builder.Services.AddScoped<IDropItemsService, DropItemsService>();
builder.Services.AddScoped<ICssStyleService, CssStyleService>();

builder.Services.AddScoped<IPrintInvoiceRepository, PrintInvoiceRepository>();
builder.Services.AddScoped<IPrintInvoiceService, PrintInvoiceService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
