using Microsoft.EntityFrameworkCore;
using Cafeteria.Models.Entities;

namespace Cafeteria.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Insumo> Insumos { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
    }
}