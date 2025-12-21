using FluentValidation;

namespace Basket.Application.CQRS.Basket.Commands.StoreBasket
{
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName không được bỏ trống.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Giỏ hàng không được rỗng.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(x => x.ProductId).NotEmpty().WithMessage("Id sản phẩm không được bỏ trống.");
                items.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0.");
                items.RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Giá tiền không được âm.");
            });
        }
    }
}