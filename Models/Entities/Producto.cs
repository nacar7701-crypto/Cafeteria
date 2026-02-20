namespace Cafeteria.Models.Entities
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal PrecioVenta { get; set; } // El precio que paga el cliente
        public decimal CostoProduccion { get; set; } // Lo que te costó hacerlo (para reportes)

        public bool Activo { get; set; } = true;
        
        // Relación: Un producto tiene muchos ingredientes en su receta
        public virtual ICollection<Receta> Detalles { get; set; } = new List<Receta>();
    }
}