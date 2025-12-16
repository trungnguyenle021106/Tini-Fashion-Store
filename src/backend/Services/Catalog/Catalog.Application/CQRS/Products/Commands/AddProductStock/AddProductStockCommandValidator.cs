using FluentValidation;

namespace Catalog.Application.CQRS.Products.Commands.AddProductStock
{
    public class AddProductStockCommandValidator : AbstractValidator<AddProductStockCommand>
    {
        public AddProductStockCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id sản phẩm không được bỏ trống.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Số lượng thêm vào phải lớn hơn 0.");
        }
    }
}