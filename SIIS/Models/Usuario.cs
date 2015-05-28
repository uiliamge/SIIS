using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIIS.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Required]
        [Display(Name = "Código")]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Nome")]
        public int Nome { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public TipoUsuarioEnum TipoUsuario { get; set; }

        public virtual Responsavel Responsavel { get; set; }

        public virtual Paciente Paciente { get; set; }
    }
}