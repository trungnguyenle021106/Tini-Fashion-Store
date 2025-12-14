using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            //config.Scan(Assembly.GetExecutingAssembly()); Đợi khi nào nâng cao lên thì dùng lại

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>(); 

            return services;
        }
    }
}
