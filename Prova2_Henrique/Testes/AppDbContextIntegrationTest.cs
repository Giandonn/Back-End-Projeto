using Microsoft.EntityFrameworkCore;
using Prova.Data;  // Verifique se o namespace correto está sendo utilizado
using Prova.Models;
using System;  // Necessário para a interface IDisposable
using Xunit;

namespace ProvaTestes
{
    public class AppDbContextTests : IDisposable
    {
        private readonly AppDbContext _context;

        // Construtor único para inicialização do contexto
        public AppDbContextTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=UsuarioDB;ConnectRetryCount=0;")
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated(); // Garante que o banco de dados seja criado
        }

        [Fact]
        public void AddUserTest()
        {
            var existingUser = _context.Usuarios.FirstOrDefault(u => u.Nome == "lucas teste");

            if (existingUser != null)
            {
                _context.Usuarios.Remove(existingUser);
                _context.SaveChanges();
            }

            var newUser = new Usuario
            {
                Nome = "Lucas",
                Sobrenome = "Teste",
                Telefone = "11111",
                Email = "teste@email.com",
                Senha = "123",
                DataNascimento = DateTime.Parse("2024-11-11T22:14:07.228Z")
            };

            _context.Usuarios.Add(newUser);
            _context.SaveChanges();

            var savedUser = _context.Usuarios.FirstOrDefault(u => u.Nome == "Lucas");
            Assert.NotNull(savedUser);
            Assert.Equal("Lucas", savedUser.Nome);
        }

        // Implementação do IDisposable para liberar recursos
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
