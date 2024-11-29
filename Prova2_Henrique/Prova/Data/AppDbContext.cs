using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         


            modelBuilder.Entity<Produto>()
                .HasOne<Marca>()
                .WithMany()
                .HasForeignKey(p => p.IdMarca)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Marca>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Produto>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();


            base.OnModelCreating(modelBuilder);
        }
    }
}
