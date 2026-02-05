using Identity.Application.Common.Interfaces;
using MediatR;

namespace Identity.Application.CQRS.Auth.Commands.VerifyEmail
{
    // Đổi kiểu trả về từ bool sang string
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, string>
    {
        private readonly IIdentityService _identityService;

        public VerifyEmailHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            // Trả về kết quả từ Service ("Success" hoặc "AlreadyVerified")
            return await _identityService.ConfirmEmailAsync(request.UserId, request.Code);
        }
    }
}