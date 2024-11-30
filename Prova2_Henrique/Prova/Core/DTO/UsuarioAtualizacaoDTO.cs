using System;
using System.Collections.Generic;
using Prova.Models;

namespace Prova.DTOs
{
    public class UsuarioAtualizacaoDTO
    {
        public string Nome { get; set; } = string.Empty; 
        public string Sobrenome { get; set; } = string.Empty; 
        public string Telefone { get; set; } = string.Empty; 
        public string Senha { get; set; } = string.Empty; 

        public List<EnderecoDTO> Enderecos { get; set; } = new List<EnderecoDTO>();

        public UsuarioAtualizacaoDTO(string nome, string sobrenome, string telefone, string senha)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Telefone = telefone;
            Senha = senha;
        }
    }
}
