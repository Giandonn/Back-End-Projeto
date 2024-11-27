using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Services;
using System.Threading.Tasks;

namespace Prova.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class listaDeProdutoPorClienteController : ControllerBase
    {
        private readonly listaDeProdutoPorClienteService _service;

        public listaDeProdutoPorClienteController(listaDeProdutoPorClienteService service)
        {
            _service = service;
        }
        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarProdutoAoCliente([FromBody] listaDeProdutosPorCliente novaAssociacao)
        {
            if (novaAssociacao == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var resultado = await _service.AddProdutoAoClienteAsync(novaAssociacao.UsuarioId, novaAssociacao.ProdutoId);
            return Ok(resultado);
        }
        [HttpGet("produtos/{usuarioId}")]
        public async Task<IActionResult> ObterProdutosPorCliente(int usuarioId)
        {
            var produtos = await _service.ObterProdutosPorClienteAsync(usuarioId);

            if (produtos == null )
            {
                return NotFound("Nenhum produto encontrado para este cliente.");
            }

            return Ok(produtos);
        }

        [HttpGet("clientes/{produtoId}")]
        public async Task<IActionResult> ObterClientesPorProduto(int produtoId)
        {
            var clientes = await _service.ObterClientesPorProdutoAsync(produtoId);

            if (clientes == null)
            {
                return NotFound("Nenhum cliente encontrado para este produto.");
            }

            return Ok(clientes);
        }

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> RemoverAssociacao(int id)
        {
            var sucesso = await _service.RemoverAssociacaoAsync(id);

            if (!sucesso)
            {
                return NotFound("Associação não encontrada.");
            }

            return Ok("Associação removida com sucesso.");
        }
    }
}
