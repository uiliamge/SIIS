using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIIS.Models
{
    /// <summary>
    /// A informação registrada em um RES como o resultado de uma ação clínica, uma
    /// observação, uma interpretação clínica, ou uma intenção.
    /// </summary>
    [Table("Entrada")]
    public class Entrada
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [MaxLength(2000)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public virtual Secao Secao { get; set; }
    }
}