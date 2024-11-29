
namespace Prova.DTOs
{
    public class UsuarioAtualizacaoDTO
    {
        public string Nome { get; set; } = string.Empty; 
        public string Sobrenome { get; set; } = string.Empty; 
        public string Telefone { get; set; } = string.Empty; 
        public string Senha { get; set; } = string.Empty;

        public string Cep { get; set; } = string.Empty;

        public UsuarioAtualizacaoDTO(string nome, string sobrenome, string telefone, string senha, string cep)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Telefone = telefone;
            Senha = senha;
            Cep = cep;
        }
    }
}
