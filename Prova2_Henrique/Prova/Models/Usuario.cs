﻿using Prova.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

public class Usuario
{
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

    public string Cep { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }

    [NotMapped]
    [JsonIgnore]
    public IFormFile? Imagem { get; set; } 

    [NotMapped]
    public string ImagemBase64 { get; set; } = string.Empty;  


    public Usuario() { }

    public Usuario(string nome, string sobrenome, string telefone, string email, string senha, DateTime dataNascimento, string cpf, string cep)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        Telefone = telefone;
        Email = email;
        Senha = senha;
        DataNascimento = dataNascimento;
        Cpf = cpf;
        Cep = cep;
    }
}
