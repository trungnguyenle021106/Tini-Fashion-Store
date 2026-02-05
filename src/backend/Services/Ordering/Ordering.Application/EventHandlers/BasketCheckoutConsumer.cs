using BuildingBlocks.Application.Messaging;
using BuildingBlocks.Core.Messaging;
using MassTransit;
using MediatR;
using Ordering.Application.CQRS.Orders.Commands.CreateOrder;

namespace Ordering.Application.EventHandlers
{
    public class BasketCheckoutConsumer : IConsumer<StockConfirmedEvent>
    {
        private readonly ISender _sender;

        public BasketCheckoutConsumer(ISender sender)
        {
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<StockConfirmedEvent> context)
        {
            var message = context.Message;

            var command = new CreateOrderCommand(
                OrderId: message.OrderId,
                UserId: message.UserId,
                TotalPrice: message.TotalPrice,
                ShippingAddress: new AddressDto(
                    message.ReceiverName,
                    message.PhoneNumber,
                    message.Street,
                    message.Ward,
                    message.City,
                    message.Note
                ),
                PaymentMethod: message.PaymentMethod,
                OrderItems: message.Items.Select(i => new OrderItemDto(
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity,
                    i.PictureUrl
                )).ToList()
            );

            await _sender.Send(command);

            Console.WriteLine($"[Ordering] Order created SUCCESSFULLY (after stock check) for User: {message.UserId}");
        }
    }
}