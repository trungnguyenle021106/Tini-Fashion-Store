using BuildingBlocks.Core.Infrastructure.Data;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Identity.Application.Common.Interfaces
{
    public interface IIdentityDbContext : IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
    }
}
