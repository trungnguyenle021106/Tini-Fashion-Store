using BuildingBlocks.Core.Infrastructure.EFCore;
using Catalog.Application.Data;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.EFCore
{
    public class ProductRepository : BaseRepositoryEFcore<Product, CatalogDbContext>, IProductRepository
    {
        public ProductRepository(CatalogDbContext context) : base(context)
        {
            // Base constructor sẽ gán _context và _dbSet
        }

        // Triển khai phương thức đặc thù được định nghĩa trong IProductRepository     
    }
}
