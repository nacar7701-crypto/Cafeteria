using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Data; // Asegúrate de que este sea el namespace de tu ApplicationDbContext
using Cafeteria.Models.ViewModels;

namespace Cafeteria.Controllers
{
    public class CocinaController : Controller
    {
        // 1. Declarar la variable del contexto
        private readonly ApplicationDbContext _context;

        // 2. Inyectar el contexto en el constructor
        public CocinaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pedidos = _context.DetalleVentas
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .Where(d => d.EstadoCocina == "Pendiente" && d.Producto.ParaCocina == true)
                .OrderBy(d => d.Venta.FechaVenta) // El primero es el más antiguo
                .Select(d => new CocinaMonitorVM
                {
                    DetalleId = d.DetalleVentaId, // Necesitas el ID para despachar uno solo
                    ProductoNombre = d.Producto.Nombre,
                    CantidadTotal = (int)d.Cantidad,
                    HoraPedido = d.Venta.FechaVenta
                })
                .ToList();

            return View(pedidos);
        }

        [HttpPost]
        public IActionResult CompletarPedido(int detalleId) // El nombre del parámetro debe coincidir con el 'name' del input
        {
            var detalle = _context.DetalleVentas.Find(detalleId);

            if (detalle != null)
            {
                detalle.EstadoCocina = "Completado"; // O "Listo"
                _context.Update(detalle);
                _context.SaveChanges();
            }

            // Redirige de vuelta al monitor (ajusta "Monitor" por el nombre de tu vista principal)
            return RedirectToAction("Index");
        }
    }
}