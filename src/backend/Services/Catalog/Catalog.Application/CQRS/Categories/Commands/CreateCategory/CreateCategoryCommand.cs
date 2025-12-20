using BuildingBlocks.Core.CQRS;

namespace Catalog.Application.CQRS.Categories.Commands.CreateCategory
{
    public record CreateCategoryResult(Guid Id);
    public record CreateCategoryCommand(string Name) : ICommand<CreateCategoryResult>;
}