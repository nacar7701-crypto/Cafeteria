using Cafeteria.Models.Entities;

namespace Cafeteria.Models.ViewModels
{
    public class ProductoRecetaVM
    {
        public string Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal CostoTotal { get; set; }

        // La lista debe llamarse exactamente 'Detalles' para coincidir con el JS
        public List<DetalleRecetaInput> Detalles { get; set; } = new List<DetalleRecetaInput>();

        // Para llenar el combo de selección
        public List<Insumo>? InsumosDisponibles { get; set; }
    }

    public class DetalleRecetaInput
    {
        public int InsumoId { get; set; }
        public decimal Cantidad { get; set; }
    }
}