using Microsoft.EntityFrameworkCore;
using Prova.Models;

namespace Prova.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Marca> Marcas { get; set; } // DbSet para Marca

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento Endereço-Usuário
            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Enderecos)
                .HasForeignKey(e => e.UsuarioId);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            // Relacionamento Produto-Marca (Um Produto tem uma Marca)
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Marca)         // Produto tem uma Marca
                .WithMany(m => m.Produtos)    // Marca tem muitos Produtos
                .HasForeignKey(p => p.MarcaId); // Chave estrangeira do Produto para Marca

            // Propriedades
            modelBuilder.Entity<Produto>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
