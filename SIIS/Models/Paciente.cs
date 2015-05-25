using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    [Table("Paciente")]
    public class Paciente : Pessoa
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Composicao> Composicoes { get; set; }

        public virtual ICollection<PermissaoResponsavelPaciente> PermissoesResponsavelPaciente { get; set; }
    }
}