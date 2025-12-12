using BuildingBlocks.Core.Extensions;
using Catalog.Infrastructure.Data.EFCore;
using Catalog.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddCustomMediatR(typeof(Catalog.Application.AssemblyReference).Assembly);
builder.Services.AddApplication();
var app = builder.Build();
app.Run();