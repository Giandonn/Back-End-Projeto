using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Data;
using Microsoft.EntityFrameworkCore;
using Prova.Services;

namespace Prova.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public UsuarioController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;  
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Dados do usuário não fornecidos.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExistente)
            {
                return Conflict("Já existe um usuário com este email.");
            }

            // Validar CPF
            var cpfValidar = await _context.Usuarios.AnyAsync(u => u.Cpf == usuario.Cpf);
            if (!ValidarCPF(usuario.Cpf))
            {
                return BadRequest("CPF inválido.");
            }

            try
            {
                _context.Usuarios.Add(usuario);
                if (usuario.Enderecos != null)
                {
                    foreach (var endereco in usuario.Enderecos)
                    {
                        endereco.UsuarioId = usuario.Id;
                        _context.Enderecos.Add(endereco);
                    }
                }

                await _context.SaveChangesAsync();

                var token = _jwtService.GerarToken(usuario.Nome, usuario.Email);

                return CreatedAtAction(nameof(CriarUsuario), new { id = usuario.Id }, new { usuario, token });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Erro ao salvar os dados: " + ex.Message);
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> GetUsuarioPorEmailSenha([FromQuery] string email, [FromQuery] string senha, [FromQuery] string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                return BadRequest("Email e senha são obrigatórios.");
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Enderecos)
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            string tokenStatus = _jwtService.ValidarToken(token);

            return Ok(new { usuario, tokenStatus });
        }


        private bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11) return false;
            if (cpf.All(c => c == cpf[0])) return false;

            int soma1 = 0;
            for (int i = 0; i < 9; i++) soma1 += int.Parse(cpf[i].ToString()) * (10 - i);
            int resto1 = soma1 % 11;
            int digito1 = (resto1 < 2) ? 0 : 11 - resto1;
            if (digito1 != int.Parse(cpf[9].ToString())) return false;

            int soma2 = 0;
            for (int i = 0; i < 10; i++) soma2 += int.Parse(cpf[i].ToString()) * (11 - i);
            int resto2 = soma2 % 11;
            int digito2 = (resto2 < 2) ? 0 : 11 - resto2;
            return digito2 == int.Parse(cpf[10].ToString());
        }
    }
}
