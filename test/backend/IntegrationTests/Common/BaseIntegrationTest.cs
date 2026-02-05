using IntegrationTests.Common;
using IntegrationTests.Helpers;
using System.Net.Http.Headers;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client; // Client mặc định (có Auth)
    protected readonly CustomWebApplicationFactory Factory;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Factory = factory;

        // Client mặc định luôn có Auth cho tiện lợi
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(TestAuthHandler.AuthenticationScheme);
    }

    // Helper để tạo Client sạch khi cần
    protected HttpClient CreateAnonymousClient()
    {
        return Factory.CreateClient();
    }
}