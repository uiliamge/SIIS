using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;
using System.Collections;

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

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
