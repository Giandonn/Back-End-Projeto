using Microsoft.AspNetCore.Mvc;
using Prova.DTOs;
using Prova.Services;

namespace Prova.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CriarUsuario([FromForm] UsuarioCadastroDTO usuarioDto)
        {
            try
            {
                var usuarioComToken = await _usuarioService.CriarUsuario(usuarioDto);

                return CreatedAtAction(
                    nameof(CriarUsuario),
                    new { id = usuarioComToken.Usuario.Id }, 
                    usuarioComToken
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }


        [HttpPut("{email}")]
        public async Task<IActionResult> AtualizarUsuario(string email, [FromBody] UsuarioAtualizacaoDTO usuarioAtualizado)
        {
            try
            {
                await _usuarioService.AtualizarUsuario(email, usuarioAtualizado);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> GetUsuarioPorEmailSenha([FromQuery] string email, [FromQuery] string senha, [FromQuery] string token)
        {
            try
            {
                var usuarioAutenticado = await _usuarioService.AutenticarUsuario(email, senha, token);
                return Ok(usuarioAutenticado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }
    }
}
