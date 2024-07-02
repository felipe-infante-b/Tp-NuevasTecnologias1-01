using Microsoft.EntityFrameworkCore;

namespace Biblioteca07.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}