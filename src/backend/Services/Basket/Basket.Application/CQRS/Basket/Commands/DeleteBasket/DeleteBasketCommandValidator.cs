using FluentValidation;

namespace Basket.Application.CQRS.Basket.Commands.DeleteBasket
{
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("UserName không được bỏ trống.");
        }
    }
}