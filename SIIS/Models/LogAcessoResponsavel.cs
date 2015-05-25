using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    [Table("LogAcessoResponsavel")]
    public class LogAcessoResponsavel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        
        [Required]
        public virtual Paciente Paciente { get; set; }

        [Required]
        public virtual Responsavel Responsavel { get; set; }

        public DateTime DataHora { get; set; }

        [Required]
        [Display(Name = "IP de Origem")]
        public string IP { get; set; }
    }
}