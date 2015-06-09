using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;
using System.Collections;
using System.Linq.Expressions;
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

        public IPagedList<Extrato> Listar(int idResponsavel, string codigo, string cpf, string datainicio,
            string dataFim, string nome, string cidade, string orderBy, int? pagina)
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

            Expression<Func<Extrato, bool>> expressaoNome = x =>
                (string.IsNullOrEmpty(nome) || x.Paciente == null || x.Paciente.Nome.Contains(nome));

            Expression<Func<Extrato, bool>> expressaoCidade = x =>
                (string.IsNullOrEmpty(cidade) || x.Cidade.Contains(cidade));

            #endregion

            var lista = _contexto.Extratos.Include(x => x.Composicoes)
                .Where(x => x.Responsavel.Id == idResponsavel)
                .Where(expressaoCodigo)
                .Where(expressaoCpf)
                .Where(expressaoDatas)
                .Where(expressaoNome)
                .Where(expressaoCidade);

            IPagedList<Extrato> listaPaginada = lista.OrderBy(x => orderBy).ToPagedList(pagina ?? 1, 2);
            return listaPaginada;
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
