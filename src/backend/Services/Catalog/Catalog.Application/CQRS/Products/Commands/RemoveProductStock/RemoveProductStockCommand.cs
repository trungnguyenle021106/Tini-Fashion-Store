using BuildingBlocks.Core.CQRS;

namespace Catalog.Application.CQRS.Products.Commands.RemoveProductStock
{
    public record RemoveProductStockResult(bool IsSuccess);

    public record RemoveProductStockCommand(Guid Id, int Quantity)
        : ICommand<RemoveProductStockResult>;
}