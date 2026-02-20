using Microsoft.AspNetCore.Mvc;
using Cafeteria.Models.Entities;
using Cafeteria.Models.ViewModels;
using Cafeteria.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Cafeteria.Controllers
{
    public class RecetasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecetasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Productos.ToList());

        public IActionResult Crear()
        {
            var viewModel = new ProductoRecetaVM {
                InsumosDisponibles = _context.Insumos.OrderBy(i => i.Nombre).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuardarProductoConReceta(ProductoRecetaVM model)
        {
            // SOLUCIÓN AL PUNTO DECIMAL: Forzar cultura US para procesar el punto
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Guardar Producto
                        var nuevoProducto = new Producto {
                            Nombre = model.Nombre,
                            PrecioVenta = model.PrecioVenta,
                            CostoProduccion = model.CostoTotal,
                            Activo = true
                        };
                        _context.Productos.Add(nuevoProducto);
                        _context.SaveChanges(); 

                        // 2. Guardar Detalles de Receta
                        if (model.Detalles != null && model.Detalles.Any())
                        {
                            foreach (var item in model.Detalles)
                            {
                                var receta = new Receta {
                                    ProductoId = nuevoProducto.ProductoId,
                                    InsumoId = item.InsumoId,
                                    Cantidad = item.Cantidad
                                };
                                _context.Recetas.Add(receta);
                            }
                            _context.SaveChanges();
                        }

                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Error: " + ex.Message);
                    }
                }
            }

            // Si falla, recargamos la vista con errores
            model.InsumosDisponibles = _context.Insumos.ToList();
            return View("Crear", model);
        }
    }
}