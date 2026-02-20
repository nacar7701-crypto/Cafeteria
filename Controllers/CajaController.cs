using Microsoft.AspNetCore.Mvc;
using Cafeteria.Data; // Asegúrate de tener este using para tu DBContext
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Controllers
{
    public class CajaController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inyectamos la base de datos en el constructor
        public CajaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Seguridad básica: Si no hay sesión, regresa al login
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Rol")))
                return RedirectToAction("Login", "Account");

            // 2. Traer los productos de la base de datos
            // Solo traemos los que están activos para que la cajera no venda cosas que no hay
            var productos = _context.Productos.Where(p => p.Activo).ToList();

            // 3. PASAR LOS PRODUCTOS A LA VISTA
            // Ahora 'Model' en la vista tendrá la lista y el error desaparecerá
            return View(productos);
        }

        [HttpPost]
        public IActionResult ProcesarVenta([FromBody] List<CarritoItem> carrito)
        {
            if (carrito == null || !carrito.Any()) return BadRequest("Carrito vacío");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Opcional: Registrar la Venta en una tabla Ventas (Si la tienes)
                    // var nuevaVenta = new Venta { Fecha = DateTime.Now, Total = carrito.Sum(x => x.Precio * x.Cantidad) };
                    // _context.Ventas.Add(nuevaVenta);
                    // _context.SaveChanges();

                    // 2. DESCONTAR INVENTARIO
                    foreach (var item in carrito)
                    {
                        // Buscamos la receta de este producto
                        var ingredientes = _context.Recetas
                            .Where(r => r.ProductoId == item.productoId)
                            .ToList();

                        foreach (var receta in ingredientes)
                        {
                            var insumo = _context.Insumos.Find(receta.InsumoId);
                            if (insumo != null)
                            {
                                // Cantidad a descontar = (Cantidad en receta * Cantidad de productos vendidos)
                                decimal cantidadADescontar = receta.Cantidad * item.cantidad;
                                insumo.StockActual -= cantidadADescontar;

                                _context.Insumos.Update(insumo);
                            }
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "Error al procesar: " + ex.Message);
                }
            }
        }

        // Clase auxiliar para recibir el JSON (puedes ponerla al final del archivo del controlador)
        public class CarritoItem
        {
            public int productoId { get; set; }
            public string nombre { get; set; }
            public decimal precio { get; set; }
            public int cantidad { get; set; }
        }
    }
}