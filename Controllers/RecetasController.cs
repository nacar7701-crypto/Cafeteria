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

        public IActionResult Crear()
        {
            var viewModel = new ProductoRecetaVM
            {
                InsumosDisponibles = _context.Insumos.OrderBy(i => i.Nombre).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuardarProductoConReceta(ProductoRecetaVM model)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var nuevoProducto = new Producto
                        {
                            Nombre = model.Nombre,
                            PrecioVenta = model.PrecioVenta,
                            CostoProduccion = model.CostoTotal,
                            Activo = true
                        };
                        _context.Productos.Add(nuevoProducto);
                        _context.SaveChanges();

                        if (model.Detalles != null && model.Detalles.Any())
                        {
                            foreach (var item in model.Detalles)
                            {
                                _context.Recetas.Add(new Receta
                                {
                                    ProductoId = nuevoProducto.ProductoId,
                                    InsumoId = item.InsumoId,
                                    Cantidad = item.Cantidad
                                });
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
            model.InsumosDisponibles = _context.Insumos.ToList();
            return View("Crear", model);
        }

        // --- SECCIÓN DE EDICIÓN ---

        // GET: Recetas/Editar/5
        public IActionResult Editar(int id)
        {
            var producto = _context.Productos
                .Include(p => p.Recetas) // Carga los ingredientes
                .FirstOrDefault(p => p.ProductoId == id);

            if (producto == null) return NotFound();

            var viewModel = new ProductoRecetaVM
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                PrecioVenta = producto.PrecioVenta,
                CostoTotal = producto.CostoProduccion,
                Detalles = producto.Recetas.Select(r => new DetalleRecetaInput
                {
                    InsumoId = r.InsumoId,
                    Cantidad = r.Cantidad
                }).ToList(),
                InsumosDisponibles = _context.Insumos.OrderBy(i => i.Nombre).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Actualizar(ProductoRecetaVM model)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var producto = _context.Productos.Find(model.ProductoId);
                        if (producto == null) return NotFound();

                        // 1. Actualizar datos básicos del producto
                        producto.Nombre = model.Nombre;
                        producto.PrecioVenta = model.PrecioVenta;
                        producto.CostoProduccion = model.CostoTotal;
                        _context.Update(producto);

                        // 2. Limpiar la receta anterior (Borrado físico de filas de ingredientes)
                        var detallesAnteriores = _context.Recetas.Where(r => r.ProductoId == model.ProductoId);
                        _context.Recetas.RemoveRange(detallesAnteriores);
                        _context.SaveChanges();

                        // 3. Insertar la nueva versión de la receta
                        if (model.Detalles != null)
                        {
                            foreach (var item in model.Detalles)
                            {
                                _context.Recetas.Add(new Receta
                                {
                                    ProductoId = producto.ProductoId,
                                    InsumoId = item.InsumoId,
                                    Cantidad = item.Cantidad
                                });
                            }
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Error al actualizar: " + ex.Message);
                    }
                }
            }

            model.InsumosDisponibles = _context.Insumos.ToList();
            return View("Editar", model);
        }

        // GET: Recetas
        public IActionResult Index()
        {
            // Solo mostramos productos activos
            var productos = _context.Productos.Where(p => p.Activo).ToList();
            return View(productos);
        }

        // POST: Recetas/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                // En lugar de borrar de la DB, lo desactivamos para no romper el historial de ventas
                producto.Activo = false;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}