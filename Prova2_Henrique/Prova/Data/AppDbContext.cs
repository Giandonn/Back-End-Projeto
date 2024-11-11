using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Enderecos)
                .HasForeignKey(e => e.UsuarioId);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
