using System.ComponentModel.DataAnnotations;

namespace Cafeteria.Models.Entities
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }
        
        [Required]
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        
        public decimal Total { get; set; }
        
        // Relación con los detalles
        public virtual ICollection<DetalleVenta> Detalles { get; set; }
    }
}