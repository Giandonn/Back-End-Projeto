using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prova.Models {
	public class Usuario {
		[Key]
		[JsonIgnore]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string Nome { get; set; } = string.Empty;
		public string Sobrenome { get; set; } = string.Empty;
		public string Telefone { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Senha { get; set; } = string.Empty;
		public string Cpf { get; set; } = string.Empty;

		public string Imagem { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        [NotMapped] 
        public string DataNascimentoFormatada => DataNascimento.ToString("dd MMM yyyy");

        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();

		public Usuario() { } 

		public Usuario(string nome, string sobrenome, string telefone, string email, string senha, DateTime dataNascimento, string cpf, string imagem) {
			Nome = nome;
			Sobrenome = sobrenome;
			Telefone = telefone;
			Email = email;
			Senha = senha;
            DataNascimento = dataNascimento;
			Cpf = cpf;
			Imagem = imagem;

        }
	}
}
