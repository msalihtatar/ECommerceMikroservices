using BasketAPI.Dtos;
using Core.Dtos;
using System.Text.Json;

namespace BasketAPI.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }
        
        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var isExistBasket = await _redisService.GetDatabase().StringGetAsync(userId);

            if (string.IsNullOrEmpty(isExistBasket)) 
            {
                return Response<BasketDto>.Fail("Sepet bulunamadı.", 404);
            }
            
            var basket = JsonSerializer.Deserialize<BasketDto>(isExistBasket);

            return Response<BasketDto>.Success(basket, 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basket)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket could not update or save", 500);
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDatabase().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 404);
        }
    }
}
