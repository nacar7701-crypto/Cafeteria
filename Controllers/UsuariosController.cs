using Microsoft.AspNetCore.Mvc;
using Cafeteria.Data;
using Cafeteria.Models.Entities;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsuariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("registrar")]
    public IActionResult Registrar(string nombre, string user, string pass, string rol)
    {
        var nuevoUsuario = new Usuario
        {
            NombreCompleto = nombre,
            Username = user,
            // Aquí encriptamos la contraseña que tú escribas
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(pass), 
            Rol = rol,
            Activo = true
        };

        _context.Usuarios.Add(nuevoUsuario);
        _context.SaveChanges();
        return Ok("Usuario creado con éxito");
    }
}