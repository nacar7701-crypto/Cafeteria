using Microsoft.AspNetCore.Mvc;
using Cafeteria.Data;
using Cafeteria.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cafeteria.Controllers
{
    public class InsumosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsumosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listado de Insumos (Esto es lo que verás al dar clic en Inventario)
        public IActionResult Index()
        {
            var listaInsumos = _context.Insumos.ToList();
            return View(listaInsumos);
        }

        public IActionResult Crear() => View();

        [HttpPost]
        public IActionResult Crear(Insumo insumo)
        {
            if (ModelState.IsValid)
            {
                _context.Insumos.Add(insumo);
                _context.SaveChanges();
                // Al guardar, nos regresa a la lista (Index)
                return RedirectToAction("Index");
            }
            return View(insumo);
        }
        // ... dentro de InsumosController ...

        public IActionResult Editar(int id)
        {
            var insumo = _context.Insumos.Find(id);
            return View(insumo);
        }

        [HttpPost]
        public IActionResult Editar(Insumo insumo, string motivo)
        {
            // Usamos AsNoTracking para comparar el valor viejo con el nuevo
            var original = _context.Insumos.AsNoTracking().FirstOrDefault(x => x.InsumoId == insumo.InsumoId);

            if (ModelState.IsValid && original != null)
            {
                // Guardamos el historial
                _context.Database.ExecuteSqlInterpolated($@"
            INSERT INTO HistorialInsumos (InsumoId, CantidadAnterior, CantidadNueva, Motivo, Usuario)
            VALUES ({insumo.InsumoId}, {original.StockActual}, {insumo.StockActual}, {motivo}, {HttpContext.Session.GetString("Nombre")})");

                _context.Insumos.Update(insumo);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insumo);
        }

        public IActionResult Eliminar(int id)
        {
            var insumo = _context.Insumos.Find(id);
            if (insumo != null)
            { // Esto quita el warning del Remove
                _context.Insumos.Remove(insumo);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}