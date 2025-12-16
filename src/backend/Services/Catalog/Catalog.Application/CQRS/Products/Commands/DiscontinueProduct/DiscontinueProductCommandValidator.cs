using FluentValidation;

namespace Catalog.Application.CQRS.Products.Commands.DiscontinueProduct
{
    public class DiscontinueProductCommandValidator : AbstractValidator<DiscontinueProductCommand>
    {
        public DiscontinueProductCommandValidator()
        {
            RuleFor(x => x.Id)
             .NotEmpty().WithMessage("Id sản phẩm không được bỏ trống.");
        }
    }
}