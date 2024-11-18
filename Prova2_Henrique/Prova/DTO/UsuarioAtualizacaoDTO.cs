using System;
using System.Collections.Generic;
using Prova.Models;  // Certifique-se de que esta importação está correta para o seu projeto.

namespace Prova.DTOs
{
    public class UsuarioAtualizacaoDTO
    {
        public string Nome { get; set; } = string.Empty;  // Valor padrão
        public string Sobrenome { get; set; } = string.Empty;  // Valor padrão
        public string Telefone { get; set; } = string.Empty;  // Valor padrão
        public string Senha { get; set; } = string.Empty;  // Valor padrão

        // Adicionando a propriedade Enderecos
        public List<EnderecoDTO> Enderecos { get; set; } = new List<EnderecoDTO>();

        // Construtor (se necessário)
        public UsuarioAtualizacaoDTO(string nome, string sobrenome, string telefone, string senha)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Telefone = telefone;
            Senha = senha;
        }
    }
}
