using FluentValidation;

namespace Basket.Application.CQRS.Basket.Commands.DeleteBasket
{
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId không được bỏ trống.");
        }
    }
}