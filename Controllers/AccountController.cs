using Microsoft.AspNetCore.Mvc;
using Cafeteria.Data;
using Cafeteria.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Cafeteria.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Username == model.Usuario);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                HttpContext.Session.SetString("Usuario", user.Username);
                HttpContext.Session.SetString("Rol", user.Rol);
                HttpContext.Session.SetString("Nombre", user.NombreCompleto);

                // Redirección por Rol
                return user.Rol switch
                {
                    "Administrador" => RedirectToAction("Index", "Admin"),
                    "Cajero" => RedirectToAction("Index", "Caja"),
                    "Cocina" => RedirectToAction("Index", "Cocina"),
                    _ => RedirectToAction("Login")
                };
            }
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}