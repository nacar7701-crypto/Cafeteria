using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Models.Entities
{
    public class DetalleVenta
    {
        [Key]
        public int DetalleVentaId { get; set; }

        [Required]
        public int VentaId { get; set; }
        public virtual Venta Venta { get; set; }

        [Required]
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }

        [Required]
        public decimal Cantidad { get; set; }
        
        public decimal PrecioUnitario { get; set; }

        // Este campo es el que usará el cocinero para "despachar"
        [Required]
        public string EstadoCocina { get; set; } = "Pendiente"; // Pendiente, En Proceso, Listo
    }
}