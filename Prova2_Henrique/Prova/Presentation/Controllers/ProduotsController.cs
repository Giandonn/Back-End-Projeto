using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prova.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllProdutos()
        {
            var produtos = await _produtoService.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> CreateProduto(Produto produto)
        {
            var newProduto = await _produtoService.AddAsync(produto);
            return CreatedAtAction(nameof(GetProdutoById), new { id = newProduto.Id }, newProduto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllProdutos()
        {
            await _produtoService.DeleteAllAsync();
            return NoContent();
        }

        [HttpPut("{nome}")]
        public async Task<IActionResult> AtualizarPorNome(string nome, [FromBody] Produto produtoAtualizado)
        {
            if (string.IsNullOrWhiteSpace(nome) || produtoAtualizado == null)
            {
                return BadRequest(new { mensagem = "Nome ou dados inválidos para atualização." });
            }

            var produto = await _produtoService.UpdateByNameAsync(nome, produtoAtualizado);

            if (produto == null)
            {
                return NotFound(new { mensagem = "Produto com o nome especificado não foi encontrado." });
            }

            return Ok(new { mensagem = "Produto atualizado com sucesso!", produto });
        }
    }
}
