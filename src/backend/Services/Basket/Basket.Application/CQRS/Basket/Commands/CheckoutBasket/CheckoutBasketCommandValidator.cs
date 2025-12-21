using FluentValidation;

namespace Basket.Application.CQRS.Basket.Commands.CheckoutBasket
{
    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tên đăng nhập không được để trống");

            RuleFor(x => x.ReceiverName)
                .NotEmpty().WithMessage("Tên người nhận không được để trống.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại là bắt buộc.")
                .Matches(@"^\d{10,11}$").WithMessage("Số điện thoại không hợp lệ.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Địa chỉ (số nhà, đường) là bắt buộc.");

            RuleFor(x => x.Ward)
                .IsInEnum().WithMessage("Phường/Xã không hợp lệ.");
        }
    }
}