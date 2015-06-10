﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;
using System.Collections;
using System.Linq.Expressions;
using System.Web.Mvc;
using PagedList;

namespace SIIS.Negocio
{
    public class ExtratoNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();

        public void Inserir(Extrato extrato)
        {
            PacienteNeg pacienteNeg = new PacienteNeg(_contexto);
            ResponsavelNeg responsavelNeg = new ResponsavelNeg(_contexto);

            extrato.DataHora = DateTime.Now;
            extrato.IndImportado = 0;
            extrato.Cidade = extrato.Responsavel.Cidade;
            extrato.Uf = extrato.Responsavel.Uf;

            extrato.Paciente = pacienteNeg.BuscarPorCpf(extrato.CpfPaciente);

            extrato.Responsavel = responsavelNeg.Buscar(extrato.Responsavel.Id);

            _contexto.Extratos.Add(extrato);
        }

        public void Editar(Extrato extrato)
        {
            ComposicaoNeg composicaoNeg = new ComposicaoNeg(_contexto);
            PacienteNeg pacienteNeg = new PacienteNeg(_contexto);

            composicaoNeg.Editar(extrato.Composicoes.Where(x => x.Id > 0), extrato.Id);

            var novaComposicao = extrato.Composicoes.FirstOrDefault(x => x.Id == 0);
            if (novaComposicao != null)
            {
                novaComposicao.Extrato = _contexto.Extratos.Single(x => x.Id == extrato.Id);
                _contexto.Composicoes.Add(novaComposicao);
            }

            extrato.DataHora = DateTime.Now;
            var old = _contexto.Extratos.FirstOrDefault(x => x.Id == extrato.Id);

            if (old != null)
            {
                if (old.CpfPaciente != extrato.CpfPaciente)
                    extrato.Paciente = pacienteNeg.BuscarPorCpf(extrato.CpfPaciente);

                _contexto.Entry(old).CurrentValues.SetValues(extrato);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public Extrato Buscar(int id)
        {
            return _contexto.Extratos.Include(x => x.Composicoes).FirstOrDefault(x => x.Id == id);
        }

        public IPagedList<Extrato> Listar(int? idResponsavel, string codigo, string cpf, string datainicio,
            string dataFim, string nome, string cidade, string plano, string orderBy, string tipoOrderBy, int? pagina)
        {
            #region expressões
            DateTime dataInicial = new DateTime(1900, 1, 1);
            DateTime dataFinal = new DateTime(9999, 1, 1);

            if (!string.IsNullOrEmpty(datainicio))
                dataInicial = Convert.ToDateTime(datainicio);
            if (!string.IsNullOrEmpty(dataFim))
                dataFinal = Convert.ToDateTime(dataFim);

            Expression<Func<Extrato, bool>> expressaoResponsavel = x =>
                (idResponsavel != null ? x.Responsavel.Id == idResponsavel : true);

            Expression<Func<Extrato, bool>> expressaoCodigo = x =>
                (string.IsNullOrEmpty(codigo) || x.Id.ToString().Contains(codigo));

            Expression<Func<Extrato, bool>> expressaoCpf = x =>
                (string.IsNullOrEmpty(cpf) || x.CpfPaciente.Contains(cpf));

            Expression<Func<Extrato, bool>> expressaoDatas = x =>
                x.DataReferencia >= dataInicial && x.DataReferencia <= dataFinal;

            Expression<Func<Extrato, bool>> expressaoNome = x =>
                (string.IsNullOrEmpty(nome) || x.Paciente == null || x.Paciente.Nome.Contains(nome));

            Expression<Func<Extrato, bool>> expressaoCidade = x =>
                (string.IsNullOrEmpty(cidade) || x.Cidade.Contains(cidade));

            Expression<Func<Extrato, bool>> expressaoPlano = x =>
                (string.IsNullOrEmpty(plano) || x.PlanoSaude.ToLower().Contains(plano.ToLower()));

            #endregion

            var lista = _contexto.Extratos.Include(x => x.Composicoes)
                .Where(expressaoResponsavel)
                .Where(expressaoCodigo)
                .Where(expressaoCpf)
                .Where(expressaoDatas)
                .Where(expressaoNome)
                .Where(expressaoCidade)
                .Where(expressaoPlano);

            #region orderBy

            switch (orderBy)
            {
                case "Id": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
                case "DataReferencia": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.DataReferencia) : lista.OrderByDescending(x => x.DataReferencia); break;
                case "CpfPaciente": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.CpfPaciente) : lista.OrderByDescending(x => x.CpfPaciente); break;
                case "PlanoSaude": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.PlanoSaude) : lista.OrderByDescending(x => x.PlanoSaude); break;
                case "Cidade": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Cidade) : lista.OrderByDescending(x => x.Cidade); break;
                case "ValorCobrado": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.ValorCobrado) : lista.OrderByDescending(x => x.ValorCobrado); break;
                default: lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
            }
            #endregion

            IPagedList<Extrato> listaPaginada = lista.ToPagedList(pagina ?? 1, 8);
            return listaPaginada;
        }

        public IPagedList<Extrato> Listar(string codigo, string cpf, string datainicio, string dataFim, string cidade,
            string responsavel, string orderBy, string tipoOrderBy, int? pagina)
        {
            #region expressões
            DateTime dataInicial = new DateTime(1900, 1, 1);
            DateTime dataFinal = new DateTime(9999, 1, 1);

            if (!string.IsNullOrEmpty(datainicio))
                dataInicial = Convert.ToDateTime(datainicio);
            if (!string.IsNullOrEmpty(dataFim))
                dataFinal = Convert.ToDateTime(dataFim);

            Expression<Func<Extrato, bool>> expressaoCodigo = x =>
                (string.IsNullOrEmpty(codigo) || x.Id.ToString().Contains(codigo));

            Expression<Func<Extrato, bool>> expressaoCpf = x =>
                (string.IsNullOrEmpty(cpf) || x.CpfPaciente.Contains(cpf));

            Expression<Func<Extrato, bool>> expressaoDatas = x =>
                x.DataReferencia >= dataInicial && x.DataReferencia <= dataFinal;

            Expression<Func<Extrato, bool>> expressaoCidade = x =>
                (string.IsNullOrEmpty(cidade) || x.Cidade.Contains(cidade));

            Expression<Func<Extrato, bool>> expressaoResponsavel = x =>
                (string.IsNullOrEmpty(responsavel) || x.Responsavel.NumeroConselhoRegional.ToString().Contains(responsavel));

            #endregion

            var lista = _contexto.Extratos.Include(x => x.Composicoes)
                .Where(expressaoCodigo)
                .Where(expressaoCpf)
                .Where(expressaoDatas)
                .Where(expressaoCidade)
                .Where(expressaoResponsavel);

            #region orderBy

            switch (orderBy)
            {
                case "Id": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
                case "DataReferencia": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.DataReferencia) : lista.OrderByDescending(x => x.DataReferencia); break;
                case "CpfPaciente": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.CpfPaciente) : lista.OrderByDescending(x => x.CpfPaciente); break;
                case "Responsavel.NumeroConselhoRegional": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Responsavel.NumeroConselhoRegional) : lista.OrderByDescending(x => x.Responsavel.NumeroConselhoRegional); break;
                case "Cidade": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Cidade) : lista.OrderByDescending(x => x.Cidade); break;
                default: lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
            }
            #endregion

            IPagedList<Extrato> listaPaginada = lista.ToPagedList(pagina ?? 1, 8);
            return listaPaginada;
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
