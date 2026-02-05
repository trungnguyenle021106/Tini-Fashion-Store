using BuildingBlocks.Application.Messaging;
using BuildingBlocks.Core.Messaging;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.EventHandlers;
using Catalog.Domain.Entities;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using MockQueryable.Moq; // Đảm bảo đã cài thư viện này
using Moq;
using Xunit;

namespace UnitTests.CatalogTests.UpdateProductStockConsumerTest
{
    public class UpdateProductStockConsumerTest
    {
        private readonly Mock<ICatalogDbContext> _dbContextMock;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly UpdateProductStockConsumer _consumer;

        public UpdateProductStockConsumerTest()
        {
            _dbContextMock = new Mock<ICatalogDbContext>();
            _cacheMock = new Mock<IDistributedCache>();
            _consumer = new UpdateProductStockConsumer(_dbContextMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task Consume_ShouldDeductStock_And_PublishStockConfirmed_WhenSuccess()
        {
            // Arrange
            // 1. Mock Transaction
            var transactionMock = new Mock<IDbContextTransaction>();

            // [SỬA 1]: Xóa It.IsAny<CancellationToken>() vì Interface của bạn không có tham số này
            _dbContextMock.Setup(x => x.BeginTransactionAsync())
                          .ReturnsAsync(transactionMock.Object);

            // 2. Mock Data (Sản phẩm có trong kho)
            // [SỬA 2]: Dùng Constructor công khai thay vì Object Initializer {} vì các property là private set
            var categoryId = Guid.NewGuid();
            var product = new Product("Test Product", 100, "Description", "image.url", categoryId);

            // Mặc định Constructor tạo Quantity = 1, ta thêm 9 để tổng là 10
            product.AddStock(9);

            // Lấy Id thật từ object vừa tạo (vì Id được tạo bên trong Constructor/BaseEntity)
            var productId = product.Id;

            // Mock DbSet (Sử dụng MockQueryable.Moq)
            var products = new List<Product> { product };
            var dbSetMock = products.AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(x => x.Products).Returns(dbSetMock.Object);

            // 3. Mock Context cho Consumer (MassTransit)
            var consumeContextMock = new Mock<ConsumeContext<BasketCheckoutEvent>>();
            consumeContextMock.Setup(x => x.Message).Returns(new BasketCheckoutEvent
            {
                // [SỬA 3]: Truyền đúng productId lấy từ biến product ở trên
                Items = new List<BasketCheckoutOrderItemModel> {
                    new BasketCheckoutOrderItemModel(productId, "Test Product", 100, 2, "image.url") // Mua 2 cái
                },

                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Email = "test@mail.com",
                TotalPrice = 200
            });

            // Act
            await _consumer.Consume(consumeContextMock.Object);

            // Assert
            product.Quantity.Should().Be(8);

            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            transactionMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

            consumeContextMock.Verify(x => x.Publish(
                It.IsAny<StockConfirmedEvent>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}