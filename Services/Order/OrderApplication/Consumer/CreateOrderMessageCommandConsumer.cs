using Core.Messages;
using MassTransit;
using OrderDomain.OrderAggregate;
using OrderInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAddress = new Address(context.Message.Province, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);

            Order order = new Order(context.Message.BuyerId, newAddress);

            context.Message.OrderItems.ForEach(item =>
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.Price, item.PictureUrl);
            });

            await _orderDbContext.Orders.AddAsync(order);

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
