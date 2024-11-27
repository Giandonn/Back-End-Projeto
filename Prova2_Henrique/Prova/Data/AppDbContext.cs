using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<listaDeProdutosPorCliente> ListaDeProdutosPorClientes { get; set; }

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

            modelBuilder.Entity<listaDeProdutosPorCliente>()
                .HasOne(lp => lp.Usuario)
                .WithMany()
                .HasForeignKey(lp => lp.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<listaDeProdutosPorCliente>()
                .HasOne(lp => lp.Produto)
                .WithMany()
                .HasForeignKey(lp => lp.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<listaDeProdutosPorCliente>()
                .Property(lp => lp.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
