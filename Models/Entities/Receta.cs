namespace Cafeteria.Models.Entities
{
    public class Receta
    {
        public int RecetaId { get; set; }
        
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }

        public int InsumoId { get; set; }
        public virtual Insumo Insumo { get; set; }

        public decimal Cantidad { get; set; } // Ejemplo: 0.100 (para 100g de tortilla)
    }
}