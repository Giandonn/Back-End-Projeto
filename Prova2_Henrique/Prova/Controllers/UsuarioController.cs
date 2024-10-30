using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Prova.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class UsuarioController : ControllerBase {
		private readonly AppDbContext _context;

		public UsuarioController(AppDbContext context) {
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> CriarUsuario([FromBody] Usuario usuario) {
			if (usuario == null) {
				return BadRequest("Dados do usu�rio n�o fornecidos.");
			}

			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			var emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
			if (emailExistente) {
				return Conflict("J� existe um usu�rio com este email.");
			}

			try {
				_context.Usuarios.Add(usuario);
				if (usuario.Enderecos != null) {
					foreach (var endereco in usuario.Enderecos) {
						endereco.UsuarioId = usuario.Id;
						_context.Enderecos.Add(endereco);
					}
				}
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(CriarUsuario), new { id = usuario.Id }, usuario);
			}
			catch (DbUpdateException ex) {
				return StatusCode(500, "Erro ao salvar os dados: " + ex.Message);
			}
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] Usuario usuarioAtualizado) {
			if (usuarioAtualizado == null) {
				return BadRequest("Dados do usu�rio n�o fornecidos.");
			}

			var usuarioExistente = await _context.Usuarios.FindAsync(id);
			if (usuarioExistente == null) {
				return NotFound("Usu�rio n�o encontrado.");
			}

			usuarioExistente.Nome = usuarioAtualizado.Nome;
			usuarioExistente.Sobrenome = usuarioAtualizado.Sobrenome;
			usuarioExistente.Telefone = usuarioAtualizado.Telefone;
			usuarioExistente.Email = usuarioAtualizado.Email;
			usuarioExistente.Senha = usuarioAtualizado.Senha;

			if (usuarioAtualizado.Enderecos != null && usuarioAtualizado.Enderecos.Count > 0) {
				var enderecosExistentes = await _context.Enderecos.Where(e => e.UsuarioId == usuarioExistente.Id).ToListAsync();
				_context.Enderecos.RemoveRange(enderecosExistentes);

				foreach (var endereco in usuarioAtualizado.Enderecos) {
					if (string.IsNullOrEmpty(endereco.Cep) || string.IsNullOrEmpty(endereco.Cidade) || string.IsNullOrEmpty(endereco.Estado)) {
						return BadRequest("Dados de endere�o inv�lidos.");
					}

					endereco.UsuarioId = usuarioExistente.Id; 
					_context.Enderecos.Add(endereco);
				}
			}

			await _context.SaveChangesAsync(); 

			return NoContent(); 
		}

        [HttpGet("login")]
        public async Task<IActionResult> GetUsuarioPorEmailSenha([FromQuery] string email, [FromQuery] string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                return BadRequest("Email e senha s�o obrigat�rios.");
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Enderecos)
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                return Unauthorized("Email ou senha inv�lidos.");
            }

            return Ok(usuario); // Use Ok() para retornar o usuario
        }

    }
}