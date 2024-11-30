using System;

namespace Prova.DTOs
{
    public class EnderecoDTO
    {
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }
}