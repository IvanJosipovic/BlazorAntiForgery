using BlazorAntiForgery.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAntiForgery.Server.Controllers
{
    [ApiController]
    [AutoValidateAntiforgeryToken]
    [Route("api/[controller]")]
    public class AntiForgeryTestController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(Model model)
        {
            return Ok(model.Value);
        }
    }
}
