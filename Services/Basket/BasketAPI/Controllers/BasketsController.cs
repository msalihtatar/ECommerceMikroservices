﻿using BasketAPI.Dtos;
using BasketAPI.Services;
using Core.ControllerBases;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly IIdentityService _identityService;

        public BasketsController(IBasketService basketService, IIdentityService identityService)
        {
            _basketService = basketService;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket() 
        {
            var result = await _basketService.GetBasket(_identityService.GetUserID);
            return CreateActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basket)
        {
            basket.UserId = _identityService.GetUserID;
            var response = await _basketService.SaveOrUpdate(basket);

            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResultInstance(await _basketService.Delete(_identityService.GetUserID));
        }
    }
}
