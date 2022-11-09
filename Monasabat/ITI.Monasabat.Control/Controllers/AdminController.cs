using Microsoft.AspNetCore.Mvc;

namespace ITI.Monasabat.Control.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
