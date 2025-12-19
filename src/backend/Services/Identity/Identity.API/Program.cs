using BuildingBlocks.Core.Extensions;
using Carter;
using Identity.Application.Common.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomDbContext<IdentityDbContext, IIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});
builder.Services.AddCustomExceptionHandler();
builder.Services.AddCustomMapster(typeof(Identity.Application.AssemblyReference).Assembly);
builder.Services.AddCustomMediatR(typeof(Identity.Application.AssemblyReference).Assembly);
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

builder.Services.AddCarter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };


    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("access_token"))
            {
                context.Token = context.Request.Cookies["access_token"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllDev", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCustomExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseCors("AllowAllDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.Run();