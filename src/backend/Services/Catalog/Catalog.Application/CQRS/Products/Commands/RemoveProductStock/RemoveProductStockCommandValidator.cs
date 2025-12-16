using FluentValidation;

namespace Catalog.Application.CQRS.Products.Commands.RemoveProductStock
{
    public class RemoveProductStockCommandValidator : AbstractValidator<RemoveProductStockCommand>
    {
        public RemoveProductStockCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id sản phẩm không được bỏ trống.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Số lượng cần lấy ra phải lớn hơn 0.");
        }
    }
}