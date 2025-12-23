using Basket.Application.Common.Interfaces;
using Basket.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        private string GetBasketKey(Guid userId) => $"basket:{userId}";

        public async Task<ShoppingCart> GetBasketAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var key = GetBasketKey(userId);
            var basketJson = await _redisCache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(basketJson))
            {
                return new ShoppingCart(userId);
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basketJson)!;
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var key = GetBasketKey(basket.UserId);
            var basketJson = JsonSerializer.Serialize(basket);

            await _redisCache.SetStringAsync(key, basketJson, cancellationToken);

            return basket; 
        }

        public async Task<bool> DeleteBasketAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var key = GetBasketKey(userId);
            await _redisCache.RemoveAsync(key, cancellationToken);
            return true;
        }
    }
}