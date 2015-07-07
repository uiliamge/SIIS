using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;
using System.Collections;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Text;
using SIIS.Helpers;

namespace SIIS.Negocio
{
    public class LogAcessoNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();

        public void Inserir(LogAcessoResponsavel log, string cpfPaciente, string userIdResponsavel)
        {
            var pacienteNeg = new PacienteNeg(_contexto);
            var responsavelNeg = new ResponsavelNeg(_contexto);

            log.Paciente = pacienteNeg.BuscarPorCpf(cpfPaciente);
            log.Responsavel = responsavelNeg.BuscarPorUserId(userIdResponsavel);

            _contexto.LogsAcessoResponsavel.Add(log);
        }

        public IPagedList<LogAcessoResponsavel> Listar(int idPaciente, int? numeroConselho, string nomeResponsavel, string datainicio,
            string dataFim, string orderBy, string tipoOrderBy, int? pagina)
        {
            #region expressões
            DateTime dataInicial = new DateTime(1900, 1, 1);
            DateTime dataFinal = new DateTime(9999, 1, 1);

            if (!string.IsNullOrEmpty(datainicio))
                dataInicial = Convert.ToDateTime(datainicio);
            if (!string.IsNullOrEmpty(dataFim))
                dataFinal = Convert.ToDateTime(dataFim);

            Expression<Func<LogAcessoResponsavel, bool>> expressaoNumeroConselho = x =>
                (numeroConselho == null || x.Responsavel.NumeroConselhoRegional == numeroConselho);

            Expression<Func<LogAcessoResponsavel, bool>> expressaoDatas = x =>
                x.DataHora >= dataInicial && x.DataHora <= dataFinal;

            Expression<Func<LogAcessoResponsavel, bool>> expressaoNomeResponsavel = x =>
                (string.IsNullOrEmpty(nomeResponsavel) || x.Responsavel == null || x.Responsavel.Nome.Contains(nomeResponsavel));

            #endregion

            var lista = _contexto.LogsAcessoResponsavel
                .Include(x => x.Responsavel)
                .Include(x => x.Paciente)
                .Where(x => x.Paciente.Id == idPaciente)
                .Where(expressaoNumeroConselho)
                .Where(expressaoDatas)
                .Where(expressaoNomeResponsavel);

            #region orderBy

            switch (orderBy)
            {
                case "Data": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.DataHora) : lista.OrderByDescending(x => x.DataHora); break;
                case "NomeResponsavel": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Responsavel.Nome) : lista.OrderByDescending(x => x.Responsavel.Nome); break;
                case "NumeroConselho": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Responsavel.NumeroConselhoRegional) : lista.OrderByDescending(x => x.Responsavel.NumeroConselhoRegional); break;
                default: lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.DataHora) : lista.OrderByDescending(x => x.DataHora); break;
            }
            #endregion

            IPagedList<LogAcessoResponsavel> listaPaginada = lista.ToPagedList(pagina ?? 1, 8);
            return listaPaginada;
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
