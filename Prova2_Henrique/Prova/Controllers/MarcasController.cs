using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Marca>>> GetAllMarcas()
        {
            var marcas = await _marcaService.GetAllAsync();
            return Ok(marcas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> GetMarcaById(int id)
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
            var newMarca = await _marcaService.AddAsync(marca);
            return CreatedAtAction(nameof(GetMarcaById), new { id = newMarca.Id }, newMarca);
        }
    }
}
