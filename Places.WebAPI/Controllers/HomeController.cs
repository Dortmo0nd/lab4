using Microsoft.AspNetCore.Mvc;

namespace Places.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }
    }
}