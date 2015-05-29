using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SIIS.Models
{
    [Table("Paciente")]
    public class Paciente
    {
        public static Paciente GetByUserId(string userId)
        {
            SiteDataContext _contexto = new SiteDataContext();

            return _contexto.Pacientes.FirstOrDefault(x => x.UserId == userId);
        }

        [Display(Name = "Código")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string UserId { get; set; }

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
        public string Cep { get; set; }

        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

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

        public virtual ICollection<Composicao> Composicoes { get; set; }

        public virtual ICollection<PermissaoResponsavelPaciente> PermissoesResponsavelPaciente { get; set; }
    }
}