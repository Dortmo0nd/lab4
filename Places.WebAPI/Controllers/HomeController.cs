using Microsoft.AspNetCore.Mvc;

namespace Places.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}