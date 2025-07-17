using Microsoft.AspNetCore.Mvc;

namespace Orpheus.Controllers
{
    public class InstrumentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
