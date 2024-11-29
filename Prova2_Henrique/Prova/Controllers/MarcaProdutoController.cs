using Microsoft.AspNetCore.Mvc;
using Prova.Models; 
using Prova.Services;
using Prova.Repositories; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prova.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaProdutoController : ControllerBase
    {
        private readonly MarcaService _marcaService;
        private readonly ProdutoService _produtoService;

        public MarcaProdutoController(MarcaService marcaService, ProdutoService produtoService)
        {
            _marcaService = marcaService;
            _produtoService = produtoService;
        }

        [HttpGet("marcas")]
        public async Task<ActionResult<IEnumerable<Marca>>> GetAllMarcas()
        {
            var marcas = await _marcaService.GetAllAsync();
            return Ok(marcas);
        }

        [HttpGet("marcas/{id}")]
        public async Task<ActionResult<Marca>> GetMarcaById(int id)
        {
            var marca = await _marcaService.GetByIdAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);
        }


        [HttpPost("marcas")]
        public async Task<ActionResult<Marca>> CreateMarca(Marca marca)
        {
            var newMarca = await _marcaService.AddAsync(marca);
            return CreatedAtAction(nameof(GetMarcaById), new { id = newMarca.Id }, newMarca);
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllProdutos()
        {
            var produtos = await _produtoService.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("produtos/{id}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPost("produtos")]
        public async Task<ActionResult<Produto>> CreateProduto(Produto produto)
        {
            var newProduto = await _produtoService.AddAsync(produto);
            return CreatedAtAction(nameof(GetProdutoById), new { id = newProduto.Id }, newProduto);
        }

        [HttpDelete("produtos")]
        public async Task<IActionResult> DeleteAllProdutos()
        {
            await _produtoService.DeleteAllAsync();
            return NoContent(); 
        }

        [HttpPut("produtos/{nome}")]
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
