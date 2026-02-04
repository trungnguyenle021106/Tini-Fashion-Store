using BuildingBlocks.Application.Messaging;
using BuildingBlocks.Core.Messaging;
using Catalog.Application.Common.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application.EventHandlers
{
    public class UpdateProductStockConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ICatalogDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private const string ProductMasterKey = "product-master-key";

        public UpdateProductStockConsumer(ICatalogDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;

            using var transaction = await _dbContext.BeginTransactionAsync();

            try
            {
                var products = await _dbContext.Products
                    .Where(p => message.Items.Select(i => i.ProductId).Contains(p.Id))
                    .ToListAsync(context.CancellationToken);

                foreach (var item in message.Items)
                {
                    var product = products.Find(p => p.Id == item.ProductId);

                    if (product == null)
                    {
                        throw new Exception($"Sản phẩm {item.ProductId} không tồn tại.");
                    }

                    product.RemoveStock(item.Quantity);
                }

                await _dbContext.SaveChangesAsync(context.CancellationToken);

                // Xóa Cache
                await _cache.RemoveAsync(ProductMasterKey, context.CancellationToken);

                var confirmedEvent = new StockConfirmedEvent
                {
                    OrderId = message.OrderId,
                    UserId = message.UserId,
                    Email = message.Email,
                    TotalPrice = message.TotalPrice,
                    ReceiverName = message.ReceiverName,
                    PhoneNumber = message.PhoneNumber,
                    Street = message.Street,
                    Ward = message.Ward,
                    City = message.City,
                    Note = message.Note,
                    PaymentMethod = message.PaymentMethod,
                    Items = message.Items
                };

                await context.Publish(confirmedEvent, context.CancellationToken);

                await transaction.CommitAsync(context.CancellationToken);

                Console.WriteLine($"[Catalog] Stock deducted & Event published for OrderId: {message.OrderId}");
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync(context.CancellationToken);

                // Bắn event báo lỗi
                await context.Publish(new OrderStockRejectedEvent(message.OrderId, ex.Message), context.CancellationToken);
                Console.WriteLine($"[Catalog] Stock update FAILED for OrderId: {message.OrderId}. Reason: {ex.Message}");
            }
        }
    }
}