using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIIS.Models
{
    [Table("ConselhoRegional")]
    public class ConselhoRegional
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Sigla do Conselho Regional")]
        public string Sigla { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}