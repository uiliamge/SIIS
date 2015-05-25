using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SIIS.Models
{
    public class SiteDataContext : DbContext
    {
        public SiteDataContext() : base("DefaultConnection") { }

        public DbSet<Composicao> Composicoes { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }
        public DbSet<Extrato> Extratos { get; set; }
        public DbSet<Secao> Secoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ConselhoRegional> ConselhosRegionais { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<LogAcessoResponsavel> LogsAcessoResponsavel { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
    }
}