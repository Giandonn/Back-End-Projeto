using Prova.Models;

namespace Prova.Services
{
    internal class UsuarioRespostaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public List<Endereco> Enderecos { get; set; }
        public string ImagemBase64 { get; set; }
        public string Token { get; set; }
    }
}