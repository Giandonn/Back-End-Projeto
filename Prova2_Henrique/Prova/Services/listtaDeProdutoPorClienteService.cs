using System.Collections.Generic;
using System.Threading.Tasks;
using Prova.Models;
using Prova.Repositories;

namespace Prova.Services
{
    public class listaDeProdutoPorClienteService
    {
        private readonly listaDeProdutosPorClienteRepository _repository;

        public listaDeProdutoPorClienteService(listaDeProdutosPorClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<listaDeProdutosPorCliente> AddProdutoAoClienteAsync(int usuarioId, int produtoId)
        {
            var novaLista = new listaDeProdutosPorCliente
            {
                UsuarioId = usuarioId,
                ProdutoId = produtoId
            };

            return await _repository.AddAsync(novaLista);
        }

        public async Task<IEnumerable<listaDeProdutosPorCliente>> ObterProdutosPorClienteAsync(int usuarioId)
        {
            return await _repository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<IEnumerable<listaDeProdutosPorCliente>> ObterClientesPorProdutoAsync(int produtoId)
        {
            return await _repository.GetByProdutoIdAsync(produtoId);
        }

        public async Task<IEnumerable<listaDeProdutosPorCliente>> ListarTodosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> RemoverAssociacaoAsync(int id)
        {
            return await _repository.RemoveAsync(id);
        }
    }
}
