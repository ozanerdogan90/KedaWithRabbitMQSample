using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KedaWithRabbitMQSample.Api.Controllers
{
    [ApiController]
    [Route("health_check")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            return Ok("its ok");
        }
    }
}