using BuildingBlocks.Core.Extensions;
using Carter;
using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();

builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddScoped<ICatalogDbContext>(provider =>
    provider.GetRequiredService<CatalogDbContext>());

builder.Services.AddCustomMediatR(typeof(Catalog.Application.AssemblyReference).Assembly);
builder.Services.AddCustomMapster(typeof(Catalog.Application.AssemblyReference).Assembly);

var app = builder.Build();

app.MapCarter();

app.Run();