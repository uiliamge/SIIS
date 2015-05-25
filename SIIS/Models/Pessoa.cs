using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        [Display(Name = "CPF/CNPJ")]
        public string CpfCnpj { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "CEP")]
        public string CEP { get; set; }

        [Required]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Required]
        [Display(Name = "UF")]
        public string UF { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required]
        [Display(Name = "IP de Origem")]
        public string IP { get; set; }

        public DateTime DataHora { get; set; }
    }
}