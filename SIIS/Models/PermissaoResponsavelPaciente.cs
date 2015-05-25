using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    [Table("PermissaoResponsavelPaciente")]
    public class PermissaoResponsavelPaciente
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        
        [Required]
        public virtual Paciente Paciente { get; set; }

        [Required]
        public virtual Responsavel Responsavel { get; set; }
    }
}