using Microsoft.AspNetCore.Mvc;
using Cafeteria.Data;
using Cafeteria.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Controllers // <--- ESTO ES LO QUE FALTA
{
    [Route("[controller]")] // Esto asegura que la base sea /Venta
    public class VentaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class VentaInput {
            public int productoId { get; set; }
            public decimal precio { get; set; }
            public int cantidad { get; set; }
        }

        [HttpPost("GuardarVenta")] // Esto asegura la ruta /Venta/GuardarVenta
        public IActionResult GuardarVenta([FromBody] List<VentaInput> items)
        {
            if (items == null || !items.Any())
                return Json(new { success = false, message = "No hay productos en el ticket" });

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var nuevaVenta = new Venta
                    {
                        FechaVenta = DateTime.Now,
                        Total = items.Sum(i => i.precio * i.cantidad)
                    };

                    _context.Ventas.Add(nuevaVenta);
                    _context.SaveChanges();

                    foreach (var item in items)
                    {
                        var detalle = new DetalleVenta
                        {
                            VentaId = nuevaVenta.VentaId,
                            ProductoId = item.productoId,
                            Cantidad = item.cantidad,
                            PrecioUnitario = item.precio,
                            EstadoCocina = "Pendiente"
                        };
                        _context.DetalleVentas.Add(detalle);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return Json(new { success = true, message = "Venta guardada y enviada a cocina" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
        }
    }
}