using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Controllers
{
    public class CocinaController : Controller
    {
        public IActionResult Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Cocina")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}