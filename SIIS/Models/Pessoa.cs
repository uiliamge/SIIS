using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIIS.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Display(Name = "Código")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Id do Identity
        public string UserId { get; set; }

        [Required]
        [StringLength(14)]
        [Display(Name = "CPF")]
        public string CpfCnpj { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Display(Name = "Logradouro")]
        public string Endereco { get; set; }

        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Display(Name = "Nº")]
        public string NumeroEndereco { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        [Display(Name = "UF")]
        public UfEnum Uf { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required]
        [Display(Name = "IP de Origem")]
        public string Ip { get; set; }

        public DateTime DataHora { get; set; }
    }
}