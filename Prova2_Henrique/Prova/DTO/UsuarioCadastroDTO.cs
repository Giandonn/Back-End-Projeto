using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Prova.Models;

namespace Prova.DTOs
{
    public class UsuarioCadastroDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        [NotMapped]
        [JsonIgnore]
        public IFormFile? Imagem { get; set; }

        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();



    }
}
