using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    /// <summary>
    /// Define se é um Exame, uma Consulta, ou Documentos.
    /// O conjunto de informação registrado em um RES por um agente, como um resultado de
    /// um encontro clínico único ou sessão de documentação de registro.
    /// </summary>
    [Table("Composicao")]
    public class Composicao
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public virtual Extrato Extrato { get; set; }

        public virtual ICollection<Secao> Secoes { get; set; }
    }
}