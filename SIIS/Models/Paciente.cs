using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SIIS.Models
{
    [Table("Paciente")]
    public class Paciente : Pessoa
    {  
        [Display(Name = "Data de Nascimento")]
        public string DataNascimento { get; set; }

        [Display(Name = "Tipo de Permissão")]
        public TipoPermissaoEnum TipoPermissao { get; set; }

        public virtual ICollection<Composicao> Composicoes { get; set; }

        public virtual ICollection<PermissaoResponsavelPaciente> PermissoesResponsavelPaciente { get; set; }
        
        public static Paciente GetByUserId(string userId)
        {
            SiteDataContext _contexto = new SiteDataContext();

            return _contexto.Pacientes
                .Include(x => x.PermissoesResponsavelPaciente)
                .FirstOrDefault(x => x.UserId == userId);
        }

    }
}