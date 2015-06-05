using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIIS.Models
{
    [Table("Extrato")]
    public class Extrato
    {
        public Extrato()
        {
            Composicoes = new List<Composicao>();
        }

        [Display(Name = "Código")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Nome do Arquivo")]
        public string NomeDoArquivo { get; set; }

        /// <summary>
        /// Indicador para verificar se o arquivo foi importado.
        /// </summary>
        [Display(Name = "Arquivo Importado")]
        public byte IndImportado { get; set; }

        public DateTime DataHora { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Referência")]
        public DateTime DataReferencia { get; set; }

        [Required]
        [Display(Name = "IP de Origem")]
        public string Ip { get; set; }

        [Required]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required]
        [Display(Name = "UF")]
        public UfEnum Uf { get; set; }

        public virtual Responsavel Responsavel { get; set; }

        public virtual Paciente Paciente { get; set; }

        public virtual ICollection<Composicao> Composicoes { get; set; }
    }
}