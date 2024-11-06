using System;
using System.Collections.Generic;
using Prova.Models;

namespace Prova.DTOs
{
    public class UsuarioAtualizacaoDTO
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public DateTime DataNascimento { get; set; }
        public List<Endereco> Enderecos { get; set; }
    }
}
