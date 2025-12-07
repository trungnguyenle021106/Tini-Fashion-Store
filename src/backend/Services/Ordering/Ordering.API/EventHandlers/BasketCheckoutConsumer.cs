using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.ValueObjects;

namespace Ordering.API.EventHandlers
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ISender _sender; // Inject MediatR
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(ISender sender, ILogger<BasketCheckoutConsumer> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Processing Checkout for: {UserName}", message.UserName);

            // 1. Map Event Message -> CreateOrderCommand
            // Tạo Address dummy (vì Event của ta lúc nãy chưa có đủ field Address, bạn có thể hardcode test trước)
            var addressDto = new AddressDto(
                 "Hieu",
                 "Nguyen",
                 "email@test.com",
                 "Hanoi",
                 "Vietnam",
                 "10000"
             );

            var orderCommand = new CreateOrderCommand(new OrderDto(
                CustomerId: Guid.NewGuid(),
                OrderName: message.UserName,
                ShippingAddress: addressDto, // Truyền DTO vào
                BillingAddress: addressDto,
                OrderItems: new List<OrderItemDto>()
                {
                    new OrderItemDto("Test Product RabbitMQ", message.TotalPrice, 1)
                }
            ));

            // 2. Gửi Command qua MediatR
            var result = await _sender.Send(orderCommand);

            _logger.LogInformation("Order Created Successfully. OrderId: {OrderId}", result.Id);
        }
    }
}