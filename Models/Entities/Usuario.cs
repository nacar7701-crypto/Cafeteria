namespace Cafeteria.Models.Entities
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; } // Administrador, Cajero, Cocina
        public bool Activo { get; set; }
    }
}