using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prova.Data;
using Prova.Models;

namespace Prova.Repositories
{
    public class listaDeProdutosPorClienteRepository
    {
        private readonly AppDbContext _context;

        public listaDeProdutosPorClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        // Adicionar um produto a um cliente
        public async Task<listaDeProdutosPorCliente> AddAsync(listaDeProdutosPorCliente entity)
        {
            _context.ListaDeProdutosPorClientes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Obter todos os produtos de um cliente
        public async Task<IEnumerable<listaDeProdutosPorCliente>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.ListaDeProdutosPorClientes
                .Include(lp => lp.Usuario)
                .Include(lp => lp.Produto)
                .Where(lp => lp.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // Obter todos os clientes associados a um produto
        public async Task<IEnumerable<listaDeProdutosPorCliente>> GetByProdutoIdAsync(int produtoId)
        {
            return await _context.ListaDeProdutosPorClientes
                .Include(lp => lp.Usuario)
                .Include(lp => lp.Produto)
                .Where(lp => lp.ProdutoId == produtoId)
                .ToListAsync();
        }

        // Listar todos os registros
        public async Task<IEnumerable<listaDeProdutosPorCliente>> GetAllAsync()
        {
            return await _context.ListaDeProdutosPorClientes
                .Include(lp => lp.Usuario)
                .Include(lp => lp.Produto)
                .ToListAsync();
        }

        // Remover um registro
        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _context.ListaDeProdutosPorClientes.FindAsync(id);
            if (entity == null)
                return false;

            _context.ListaDeProdutosPorClientes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
