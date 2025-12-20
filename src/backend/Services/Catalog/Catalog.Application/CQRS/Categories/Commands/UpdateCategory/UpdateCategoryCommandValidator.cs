using FluentValidation;

namespace Catalog.Application.CQRS.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id thể loại không được bỏ trống.");

            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Tên thể loại không được để trống.")
                .MaximumLength(100).WithMessage("Tên thể loại không vượt quá 100 ký tự.");
        }
    }
}