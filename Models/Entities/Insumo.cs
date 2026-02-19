namespace Cafeteria.Models.Entities // <--- Asegúrate que diga exactamente esto
{
    public class Insumo // <--- Que no diga "Insumos" (en plural), debe ser "Insumo"
    {
        public int InsumoId { get; set; }
        public string Nombre { get; set; } = "";
        public decimal StockActual { get; set; }
        public string UnidadMedida { get; set; } = "";
        public decimal PrecioCompra { get; set; }
        public decimal StockMinimo { get; set; }
    }
}