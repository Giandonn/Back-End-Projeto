using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prova.Models
{
    public class Produto
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Preco { get; set; } = string.Empty;
        public string IdMarca { get; set; } = string.Empty;


        public Produto() { }

        public Produto(string nome, string descricao, string preco)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
        }
    }
}
