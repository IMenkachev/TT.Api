using Microsoft.AspNetCore.Mvc;

namespace TT.Api.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok(new { ok = true });
        }
    }
}
