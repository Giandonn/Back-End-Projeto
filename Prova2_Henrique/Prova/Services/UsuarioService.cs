using Prova.Repositories;
using Prova.Models;
using Prova.DTOs;
using Prova.DTO;

namespace Prova.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly JwtService _jwtService;

        public UsuarioService(UsuarioRepository usuarioRepository, JwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        public async Task<UsuarioComToken> CriarUsuario(UsuarioCadastroDTO usuarioDto)
        {
            if (usuarioDto == null)
                throw new ArgumentException("Dados do usu�rio n�o fornecidos.");

            if (!ValidarCPF(usuarioDto.Cpf))
                throw new ArgumentException("CPF inv�lido.");

            var emailExistente = await _usuarioRepository.GetPorEmailAssincrono(usuarioDto.Email);
            if (emailExistente != null)
                throw new InvalidOperationException("J� existe um usu�rio com este email.");

            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome,
                Sobrenome = usuarioDto.Sobrenome,
                Telefone = usuarioDto.Telefone,
                Senha = usuarioDto.Senha,
                Cpf = usuarioDto.Cpf,
                Email = usuarioDto.Email,
                Cep = usuarioDto.Cep
            };

            if (usuarioDto.Imagem != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await usuarioDto.Imagem.CopyToAsync(memoryStream);
                    usuario.ImagemBase64 = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            var usuarioCriado = await _usuarioRepository.AddAsync(usuario);

            var token = _jwtService.GerarToken(usuarioCriado.Nome, usuarioCriado.Email);

            return new UsuarioComToken
            {
                Token = token,
                Usuario = usuarioCriado
            };
        }

        public async Task AtualizarUsuario(string email, UsuarioAtualizacaoDTO usuarioAtualizado)
        {
            var usuarioExistente = await _usuarioRepository.GetPorEmailAssincrono(email);
            if (usuarioExistente == null)
                throw new InvalidOperationException("Usu�rio n�o encontrado.");

            usuarioExistente.Nome = usuarioAtualizado.Nome;
            usuarioExistente.Sobrenome = usuarioAtualizado.Sobrenome;
            usuarioExistente.Telefone = usuarioAtualizado.Telefone;
            usuarioExistente.Senha = usuarioAtualizado.Senha;

            // Atualizando o CEP, n�o h� mais endere�os para manipular
            usuarioExistente.Cep = usuarioAtualizado.Cep;

            await _usuarioRepository.UpdateAsync(usuarioExistente);
        }

        public async Task<Usuario> AutenticarUsuario(string email, string senha, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                throw new ArgumentException("Email e senha s�o obrigat�rios.");

            var usuario = await _usuarioRepository.GetPorEmailAssincrono(email);
            if (usuario == null || usuario.Senha != senha)
                throw new InvalidOperationException("Email ou senha inv�lidos.");

            string tokenStatus = _jwtService.ValidarToken(token);

            // N�o h� mais endere�os, ent�o vamos retornar apenas os dados b�sicos
            return new Usuario
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Cpf = usuario.Cpf,
                Cep = usuario.Cep, // Retornando o CEP
                ImagemBase64 = usuario.ImagemBase64
            };
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
