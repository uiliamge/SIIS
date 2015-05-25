using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    [Table("Responsavel")]
    public class Responsavel : Pessoa
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        
        [Required]
        public int NumeroConselho { get; set; }

        [Required]
        public virtual ConselhoRegional ConselhoRegional { get; set; }

        [Required]
        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }

        [Required]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Extrato> Extratos { get; set; }
    }
}