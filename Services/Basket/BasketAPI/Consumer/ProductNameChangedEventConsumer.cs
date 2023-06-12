using BasketAPI.Dtos;
using BasketAPI.Services;
using Core.Messages;
using Core.Services;
using MassTransit;
using System.Text.Json;

namespace BasketAPI.Consumer
{
    public class ProductNameChangedEventConsumer : IConsumer<ProductNameChangedBasketEvent>
    {
        RedisService _redisService;

        public ProductNameChangedEventConsumer(RedisService redisService, IIdentityService identityService)
        {
            _redisService = redisService;
        }

        public async Task Consume(ConsumeContext<ProductNameChangedBasketEvent> context)
        {
            var isExistBasket = await _redisService.GetDatabase().StringGetAsync(context.Message.UserId);

            if (!string.IsNullOrEmpty(isExistBasket))
            {
                var basket = JsonSerializer.Deserialize<BasketDto>(isExistBasket);

                basket.basketItems.ForEach(x =>
                {
                    x.ProductName = context.Message.UpdatedProductName;
                });

                var status = await _redisService.GetDatabase().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));
            }
        }
    }
}
