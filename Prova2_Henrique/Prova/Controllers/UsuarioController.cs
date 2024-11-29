using Microsoft.AspNetCore.Mvc;
using Prova.Models;
using Prova.Data;
using Microsoft.EntityFrameworkCore;
using Prova.Services;
using Prova.DTOs;

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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CriarUsuario([FromForm] UsuarioCadastroDTO usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Dados do usuário não fornecidos.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email);
            if (emailExistente)
            {
                return Conflict("Já existe um usuário com este email.");
            }

            if (!ValidarCPF(usuarioDto.Cpf))
            {
                return BadRequest("CPF inválido.");
            }

            try
            {

                var usuario = new Usuario
                {
                    Nome = usuarioDto.Nome,
                    Sobrenome = usuarioDto.Sobrenome,
                    Telefone = usuarioDto.Telefone,
                    Senha = usuarioDto.Senha,
                    Cpf = usuarioDto.Cpf,
                    Email = usuarioDto.Email,
                };

                if (usuarioDto.Imagem != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await usuarioDto.Imagem.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        usuario.ImagemBase64 = Convert.ToBase64String(imageBytes);
                    }
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();


                if (usuario.Enderecos != null && usuario.Enderecos.Any())
                {
                    foreach (var endereco in usuario.Enderecos)
                    {
                        if (string.IsNullOrWhiteSpace(endereco.Cep) || string.IsNullOrWhiteSpace(endereco.Cidade))
                        {
                            return BadRequest("Endereço incompleto fornecido.");
                        }

                        endereco.UsuarioId = usuario.Id;
                        _context.Enderecos.Add(endereco);
                    }
                }


                await _context.SaveChangesAsync(); 



                var token = _jwtService.GerarToken(usuario.Nome, usuario.Email);

                return CreatedAtAction(
                    nameof(CriarUsuario),
                    new { id = usuario.Id }, 
                    new
                    {
                        token,
                        usuario = new
                        {
                            usuario.Id,
                            usuario.Nome,
                            usuario.Sobrenome,
                            usuario.Telefone,
                            usuario.Email,
                            usuario.Cpf,
                            usuario.Enderecos,
                            usuario.ImagemBase64
                        }
                        
                    }
                );
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, "Erro ao salvar os dados: " + errorMessage);
            }
        }


        [HttpPut("{email}")]
        public async Task<IActionResult> AtualizarUsuario(string email, [FromBody] UsuarioAtualizacaoDTO usuarioAtualizado)
        {
            if (usuarioAtualizado == null)
            {
                return BadRequest("Dados do usu�rio n�o fornecidos.");
            }

            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuarioExistente == null)
            {
                return NotFound("Usu�rio n�o encontrado.");
            }

            usuarioExistente.Nome = usuarioAtualizado.Nome;
            usuarioExistente.Sobrenome = usuarioAtualizado.Sobrenome;
            usuarioExistente.Telefone = usuarioAtualizado.Telefone;
            usuarioExistente.Senha = usuarioAtualizado.Senha;

            if (usuarioAtualizado.Enderecos != null && usuarioAtualizado.Enderecos.Count > 0)
            {
                var enderecosExistentes = await _context.Enderecos.Where(e => e.UsuarioId == usuarioExistente.Id).ToListAsync();
                _context.Enderecos.RemoveRange(enderecosExistentes);

                foreach (var enderecoDTO in usuarioAtualizado.Enderecos)
                {
                    var endereco = new Endereco
                    {
                        Cep = enderecoDTO.Cep,
                        Cidade = enderecoDTO.Cidade,
                        Estado = enderecoDTO.Estado,
                        UsuarioId = usuarioExistente.Id
                    };

                    _context.Enderecos.Add(endereco);
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
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
