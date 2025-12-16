using FluentValidation;

namespace Catalog.Application.CQRS.Products.Commands.ActivateProduct
{
    public class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
    {
        public ActivateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                  .NotEmpty().WithMessage("Id sản phẩm không được bỏ trống.");
        }
    }
}