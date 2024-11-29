using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Prova.Models {
	public class Endereco {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[JsonIgnore]
		public int Id { get; set; }

		public string? Cep { get; set; }
		public string? Cidade { get; set; }
		public string? Estado { get; set; }

		[JsonIgnore]
		public int UsuarioId { get; set; }

		[JsonIgnore]
		public Usuario? Usuario { get; set; }

		public Endereco() { } 

		public Endereco(string cep, string cidade, string estado) {
			Cep = cep;
			Cidade = cidade;
			Estado = estado;
		}
	}
}
