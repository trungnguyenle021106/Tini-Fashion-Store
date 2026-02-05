using IntegrationTests.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ApiProgram = Basket.API.Program;

namespace IntegrationTests.Common
{
    public class CustomWebApplicationFactory : WebApplicationFactory<ApiProgram>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // 1. Cấu hình Cache
                services.RemoveAll(typeof(Microsoft.Extensions.Caching.Distributed.IDistributedCache));
                services.AddDistributedMemoryCache();

                // 2. Cấu hình MassTransit Harness
                services.AddMassTransitTestHarness();

                // 3. Cấu hình Authentication
                services.Configure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                });

                services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            TestAuthHandler.AuthenticationScheme, options => { });
            });
        }
    }
}