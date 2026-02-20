using System;
using Cafeteria.Models.Entities;

namespace Cafeteria.Models.ViewModels
{
    public class CocinaMonitorVM
    {
        public int DetalleId { get; set; }
        public string ProductoNombre { get; set; }
        public int CantidadTotal { get; set; }
        public DateTime HoraPedido { get; set; }
        public string Estado { get; set; } 
    }
}