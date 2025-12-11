using Catalog.Application.CQRS.Products.Commands.CreateProduct;
using Catalog.Domain.Entities;
using Mapster;
namespace Catalog.Application.Common.Mapping
{
    public static class MapsterExtensions
    {
        public static void RegisterProductMappings(this TypeAdapterConfig config)
        {
            config.NewConfig<CreateProductResult, Product>()
                // 1. Dùng Activator để gọi Private Constructor
                .ConstructUsing(src => (Product)Activator.CreateInstance(typeof(Product), true)!)

                // 2. Map các field
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.Quantity, src => src.Quantity)

                // 3. Xử lý field thiếu (Hard-code Guid.Empty như bạn muốn)
                .Map(dest => dest.CategoryId, src => Guid.Empty);
        }
    }
}
