using FluentValidation;

namespace Catalog.Application.CQRS.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống.");

            RuleFor(x => x.imageUrl)
                .NotEmpty().WithMessage("Hình ảnh sản phẩm không được để trống.");          
        }
    }
}
