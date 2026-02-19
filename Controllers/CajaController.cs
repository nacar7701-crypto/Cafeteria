using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Controllers
{
    public class CajaController : Controller
    {
        public IActionResult Index() 
        {
            // Seguridad básica: Si no hay sesión, regresa al login
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Rol")))
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}