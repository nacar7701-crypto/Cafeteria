using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // Verificamos que solo el Admin entre
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}