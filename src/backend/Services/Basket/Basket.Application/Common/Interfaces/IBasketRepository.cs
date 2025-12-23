using Basket.Domain.Entities;

namespace Basket.Application.Common.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasketAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}