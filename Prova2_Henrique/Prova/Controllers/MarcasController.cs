using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Services;

namespace Prova.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly MarcaService _marcaService;

        public MarcaController(MarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marca>>> GetTodasMarcas()
        {
            var marcas = await _marcaService.GetAllAsync();
            return Ok(marcas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> GetMarcaPorId(int id)
        {
            var marca = await _marcaService.GetByIdAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);
        }

        [HttpPost]
        public async Task<ActionResult<Marca>> CreateMarca(Marca marca)
        {
            var novaMarca = await _marcaService.AddAsync(marca);
            return CreatedAtAction(nameof(GetMarcaPorId), new { id = novaMarca.Id }, novaMarca);
        }
    }
}
