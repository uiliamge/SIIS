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
    public class ExtratoNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();

        public void Inserir(Extrato extrato, byte indImportado)
        {
            PacienteNeg pacienteNeg = new PacienteNeg(_contexto);
            ResponsavelNeg responsavelNeg = new ResponsavelNeg(_contexto);

            extrato.DataHora = DateTime.Now;
            extrato.IndImportado = indImportado;
            extrato.Cidade = extrato.Responsavel.Cidade;
            extrato.Uf = extrato.Responsavel.Uf;

            extrato.Paciente = pacienteNeg.BuscarPorCpf(extrato.CpfPaciente);
            if (extrato.Paciente != null)
                extrato.ExibicaoPermitida = extrato.Paciente.TipoPermissao != TipoPermissaoEnum.Perguntarme;

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
            
        public void Importar(Extrato novoExtrato, HttpPostedFileBase arquivo)
        {
            //e|01373784083|10/06/2015|unimed|120,00
            //c|Texto da primeira composição
            //s|Texto da primeira seção da primeira composição
            //s|Texto da segunda seção da primeira composição
            //c|Texto da segunda composição
            
            int counter = 0;
            int indiceComposicao = -1;
            int indiceSecao = -1;
            string linha;

            novoExtrato.IndImportado = 1;
            novoExtrato.Responsavel = Responsavel.GetByUserId(ApplicationUser.UsuarioLogado.Id);
            
            var reader = new StreamReader(arquivo.InputStream, Encoding.UTF8);
            while ((linha = reader.ReadLine()) != null)
            {
                var colunas = linha.Split('|');

                switch (colunas[0])
                {
                    case "e":
                    {
                        novoExtrato.CpfPaciente = colunas[1].FormataCpf();
                        novoExtrato.DataReferencia = Convert.ToDateTime(colunas[2]);
                        novoExtrato.PlanoSaude = colunas[3];
                        novoExtrato.ValorCobrado = colunas[4] != String.Empty ? Convert.ToDecimal(colunas[4]) : 0;
                        break;
                    }
                    case "c":
                    {
                        indiceSecao = -1;
                        indiceComposicao ++;
                        novoExtrato.Composicoes.Add(new Composicao(){ Indice = indiceComposicao, Descricao = colunas[1]});
                        break;
                    }
                    case "s":
                    {
                        indiceSecao ++;
                        novoExtrato.Composicoes.FirstOrDefault(x => x.Indice == indiceComposicao).Secoes.Add(
                            new Secao() { Indice = indiceComposicao, Descricao = colunas[1] });
                        break;
                    }
                }

                counter++;
            }
            reader.Close();
            
            Inserir(novoExtrato, 1);
        }

        public void Aprovar(int id)
        {
            var old = _contexto.Extratos.FirstOrDefault(x => x.Id == id);
            if (old != null)
            {
                var extrato = old;
                extrato.ExibicaoPermitida = true;

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

        public decimal SomarValorCobrado(int? idResponsavel, string codigo, string cpf, string datainicio,
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

            var lista = _contexto.Extratos
                .Where(expressaoResponsavel)
                .Where(expressaoCodigo)
                .Where(expressaoCpf)
                .Where(expressaoDatas)
                .Where(expressaoNome)
                .Where(expressaoCidade)
                .Where(expressaoPlano)
                .Select(x => x.ValorCobrado);

            if (lista.Any())
                return lista.Sum();

            return 0;
        }

        #region Para o Paciente
        public IPagedList<Extrato> ListarPorPaciente(int idPaciente, string codigo, string responsavel, string datainicio,
            string dataFim, string cidade, string plano, string orderBy, string tipoOrderBy, int? pagina)
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

            Expression<Func<Extrato, bool>> expressaoResponsavel = x =>
                (string.IsNullOrEmpty(responsavel) || x.Responsavel.Nome.Contains(responsavel));

            Expression<Func<Extrato, bool>> expressaoDatas = x =>
                x.DataReferencia >= dataInicial && x.DataReferencia <= dataFinal;

            Expression<Func<Extrato, bool>> expressaoCidade = x =>
                (string.IsNullOrEmpty(cidade) || x.Cidade.Contains(cidade));

            Expression<Func<Extrato, bool>> expressaoPlano = x =>
                (string.IsNullOrEmpty(plano) || x.PlanoSaude.ToLower().Contains(plano.ToLower()));

            #endregion

            var lista = _contexto.Extratos.Include(x => x.Composicoes)
                .Where(x => x.Paciente == null && x.CpfPaciente == ApplicationUser.UsuarioLogado.Cpf)
                .Where(x => x.Paciente != null && x.Paciente.Id == idPaciente)
                .Where(expressaoResponsavel)
                .Where(expressaoCodigo)
                .Where(expressaoDatas)
                .Where(expressaoCidade)
                .Where(expressaoPlano);

            #region orderBy

            switch (orderBy)
            {
                case "Id": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
                case "DataReferencia": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.DataReferencia) : lista.OrderByDescending(x => x.DataReferencia); break;
                case "Responsavel.Nome": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Responsavel.Nome) : lista.OrderByDescending(x => x.Responsavel.Nome); break;
                case "PlanoSaude": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.PlanoSaude) : lista.OrderByDescending(x => x.PlanoSaude); break;
                case "Cidade": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Cidade) : lista.OrderByDescending(x => x.Cidade); break;
                default: lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
            }
            #endregion

            IPagedList<Extrato> listaPaginada = lista.ToPagedList(pagina ?? 1, 8);
            return listaPaginada;
        }

        public IPagedList<Extrato> ListarPorAprovacao(int idPaciente, string orderBy, string tipoOrderBy, int? pagina)
        {
            var lista = _contexto.Extratos.Include(x => x.Composicoes)
                .Where(x => !x.ExibicaoPermitida)
                .Where(x => x.Paciente == null && x.CpfPaciente == ApplicationUser.UsuarioLogado.Cpf)
                .Where(x => x.Paciente != null && x.Paciente.Id == idPaciente);

            #region orderBy

            switch (orderBy)
            {
                case "Id": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
                case "DataReferencia": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.DataReferencia) : lista.OrderByDescending(x => x.DataReferencia); break;
                case "Responsavel.Nome": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Responsavel.Nome) : lista.OrderByDescending(x => x.Responsavel.Nome); break;
                case "PlanoSaude": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.PlanoSaude) : lista.OrderByDescending(x => x.PlanoSaude); break;
                case "Cidade": lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Cidade) : lista.OrderByDescending(x => x.Cidade); break;
                default: lista = tipoOrderBy == "asc" ? lista.OrderBy(x => x.Id) : lista.OrderByDescending(x => x.Id); break;
            }
            #endregion

            IPagedList<Extrato> listaPaginada = lista.ToPagedList(pagina ?? 1, 8);
            return listaPaginada;
        }
        #endregion

        /// <summary>
        /// Listagem compartilhada. Lista somente com CPFs cadastrados.
        /// </summary>
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

            var usuario = ApplicationUser.UsuarioLogado;

            var lista = _contexto.Extratos.Include(x => x.Composicoes)
                .Where(x => x.ExibicaoPermitida)
                .Where(expressaoCodigo)
                .Where(expressaoCpf)
                .Where(expressaoDatas)
                .Where(expressaoCidade)
                .Where(expressaoResponsavel)
                .Where(x => x.Paciente != null)
                .Where(x => x.Paciente.TipoPermissao != TipoPermissaoEnum.EscolherQuemPodeAcessar 
                    || x.Paciente.PermissoesResponsavelPaciente.Select(p => p.NumeroConselho).Contains(usuario.NumeroConselho));
                

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
