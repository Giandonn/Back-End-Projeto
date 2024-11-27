using Prova.Repositories;
using Prova.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prova.Services
{
    public class ProdutoService
    {
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoService(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _produtoRepository.GetAllAsync();
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            return await _produtoRepository.GetByIdAsync(id);
        }

        public async Task<Produto> AddAsync(Produto produto)
        {
            return await _produtoRepository.AddAsync(produto);
        }

        public async Task DeleteAllAsync()
        {
            await _produtoRepository.DeleteAllAsync(); 
        }
    }
}
