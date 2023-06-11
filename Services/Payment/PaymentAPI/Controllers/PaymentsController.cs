using Core.ControllerBases;
using Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomBaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment(/*PaymentDto paymentDto*/)
        {
            return CreateActionResultInstance(Response<NoContent>.Success(200));
        }
    }
}
