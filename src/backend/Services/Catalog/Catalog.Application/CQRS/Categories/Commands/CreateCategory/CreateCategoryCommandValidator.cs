using FluentValidation;

namespace Catalog.Application.CQRS.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên thể loại không được để trống.")
                .MaximumLength(100).WithMessage("Tên thể loại không vượt quá 100 ký tự.");
        }
    }
}