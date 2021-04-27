using BlazorAntiforgery.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorAntiforgery.Server.Controllers
{
    [ApiController]
    [AutoValidateAntiforgeryToken]
    [Route("api/[controller]")]
    public class AntiForgeryTestController : ControllerBase
    {
        private readonly ILogger<AntiForgeryTestController> logger;

        public AntiForgeryTestController(ILogger<AntiForgeryTestController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Post(Model model)
        {
            return Ok(model.Value);
        }
    }
}
