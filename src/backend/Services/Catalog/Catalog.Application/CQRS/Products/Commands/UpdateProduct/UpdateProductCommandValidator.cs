using FluentValidation;

namespace Catalog.Application.CQRS.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id sản phẩm không được để trống.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
                .MaximumLength(150).WithMessage("Tên sản phẩm không quá 150 ký tự.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Hình ảnh sản phẩm không được để trống.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Id thể loại không được để trống.");
        }
    }
}