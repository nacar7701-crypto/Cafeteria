using Cafeteria.Models.Entities;

namespace Cafeteria.Models.ViewModels
{
    public class ProductoRecetaVM
    {
        // ESTA ES LA PROPIEDAD QUE FALTA PARA LA EDICIÓN
        public int ProductoId { get; set; } 

        public string Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal CostoTotal { get; set; }

        public List<DetalleRecetaInput> Detalles { get; set; } = new List<DetalleRecetaInput>();

        public List<Insumo>? InsumosDisponibles { get; set; }
    }

    public class DetalleRecetaInput
    {
        public int InsumoId { get; set; }
        public decimal Cantidad { get; set; }
    }
}