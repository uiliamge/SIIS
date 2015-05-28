using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIIS.Models
{
    [Table("PermissaoResponsavelPaciente")]
    public class PermissaoResponsavelPaciente
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        
        [Required]
        public int NumeroConselho { get; set; }

        [Required]
        public ConselhoEnum SiglaConselhoRegional { get; set; }

        [Required]
        public UfEnum UfConselhoRegional { get; set; }

        [Required]
        public virtual Paciente Paciente { get; set; }
    }
}