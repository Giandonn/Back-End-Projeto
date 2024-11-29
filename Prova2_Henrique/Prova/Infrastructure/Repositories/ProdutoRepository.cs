using Prova.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prova.Infrastructure.Repositories.Data;

namespace Prova.Repositories
{
    public class ProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _context.Produtos.Include(p => p.Marca).ToListAsync();
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            return await _context.Produtos.Include(p => p.Marca).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produto> AddAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task DeleteAllAsync()
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (produtos != null && produtos.Count > 0)
            {
                _context.Produtos.RemoveRange(produtos);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Produto> UpdateByNameAsync(string nome, Produto produtoAtualizado)
        {
            var produtoExistente = await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == nome);

            if (produtoExistente == null)
            {
                return null;
            }

            produtoExistente.Nome    = produtoAtualizado.Nome;
            produtoExistente.Descricao = produtoAtualizado.Descricao;
            produtoExistente.Preco = produtoAtualizado.Preco;
           

            await _context.SaveChangesAsync();

            return produtoExistente; 
        }
    }
}
