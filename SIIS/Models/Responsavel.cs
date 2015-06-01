using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIIS.Models
{
    [Table("Responsavel")]
    public class Responsavel : Pessoa
    {
        [Required]
        public int NumeroConselhoRegional { get; set; }

        [Required]
        public ConselhoEnum ConselhoRegional { get; set; }

        [Required]
        [Display(Name = "UF")]
        public UfEnum UfConselhoRegional { get; set; }

        public virtual ICollection<Extrato> Extratos { get; set; }

        public static Responsavel GetByUserId(string userId)
        {
            SiteDataContext contexto = new SiteDataContext();

            return contexto.Responsaveis.FirstOrDefault(x => x.UserId == userId);
        }        
    }
}