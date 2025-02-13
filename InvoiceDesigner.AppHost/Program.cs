var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.InvoiceDesigner_WebApp>("invoicedesigner-webapp");

builder.AddProject<Projects.InvoiceDesigner_API>("invoicedesigner-api");

builder.Build().Run();
