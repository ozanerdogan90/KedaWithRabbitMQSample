using System.Threading;
using System.Threading.Tasks;
using KedaWithRabbitMQSample.Commands;
using Microsoft.AspNetCore.Mvc;

namespace KedaWithRabbitMQSample.Api.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CancellationToken token)
        {
            await _orderService.Execute(new OrderCommand(), token);
            return Ok();
        }
    }
}