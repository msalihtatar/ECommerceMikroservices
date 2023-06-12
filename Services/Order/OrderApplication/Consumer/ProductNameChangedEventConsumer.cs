using Core.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Consumer
{
    public class ProductNameChangedEventConsumer : IConsumer<ProductNameChangedEvent>
    {
        OrderDbContext _dbContext;
        public ProductNameChangedEventConsumer(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ProductNameChangedEvent> context)
        {
            var orderItemList = await _dbContext.OrderItems.Where(x => x.ProductId == context.Message.ProductId).ToListAsync();

            orderItemList.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.UpdatedProductName, x.PictureUrl, x.Price);
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
