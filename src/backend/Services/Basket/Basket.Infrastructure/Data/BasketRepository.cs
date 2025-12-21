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

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var basketJson = await _redisCache.GetStringAsync(userName, cancellationToken);

            if (string.IsNullOrEmpty(basketJson))
            {
                return null!;
            }

            return JsonSerializer.Deserialize<ShoppingCart>(basketJson)!;
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            var basketJson = JsonSerializer.Serialize(basket);

            await _redisCache.SetStringAsync(basket.UserName, basketJson, cancellationToken);

            return await GetBasketAsync(basket.UserName, cancellationToken);
        }

        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            await _redisCache.RemoveAsync(userName, cancellationToken);
            return true;
        }
    }
}