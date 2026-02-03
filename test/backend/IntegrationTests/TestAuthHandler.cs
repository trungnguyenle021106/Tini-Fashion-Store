using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IntegrationTests // Đảm bảo đúng namespace của bạn
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // TẠO ĐỦ CÁC LOẠI CLAIM ĐỂ "BAO SÂN" MỌI TRƯỜNG HỢP GETUSERID()
            var claims = new[] {
                new Claim(ClaimTypes.Name, "Test User"),
                
                // QUAN TRỌNG: Thêm cả 2 loại key này vì TokenProvider của bạn dùng "sub"
                // nhưng .NET đôi khi lại map nó sang NameIdentifier.
                // Khai báo cả 2 để hàm GetUserId() kiểu gì cũng bắt dính.
                new Claim(ClaimTypes.NameIdentifier, "user-id-123"),
                new Claim("sub", "user-id-123"),
                
                // Phòng hờ thêm key "id" nếu code dùng thư viện lạ
                new Claim("id", "user-id-123")
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}