using InvoiceDesigner.Application.Interfaces;
using InvoiceDesigner.Application.Services;
using InvoiceDesigner.Domain.Shared.Interfaces;
using InvoiceDesigner.Infrastructure.Data;
using InvoiceDesigner.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options => { options.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });

//	AutoMapper is used solely to convert models to DTOs.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionSqLite")));
//  create migration
//      dotnet ef migrations add AddInitialMigration --startup-project Invoicer.API --project Invoicer.Infrastructure
//  create database
//      dotnet ef database update --project Invoicer.Infrastructure --startup-project Invoicer.Api

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

builder.Services.AddScoped<IFormDesignersRepository, FormDesignersRepository>();
builder.Services.AddScoped<IFormDesignersService, FormDesignersService>();

builder.Services.AddScoped<IPrintInvoiceService, PrintInvoiceService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
