using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;

namespace SIIS.Negocio
{
    public class SecaoNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();

        public SecaoNeg()
        {
            _contexto = new SiteDataContext();
        }

        public SecaoNeg(SiteDataContext contexto)
        {
            _contexto = contexto;
        }

        public void Editar(ICollection<Secao> secoes, int idComposicao)
        {
            //Seções para excluir
            foreach (var secaoAtual in _contexto.Secoes.Where(x => x.Composicao.Id == idComposicao).ToList())
            {
                if (!secoes.Select(x => x.Id).Contains(secaoAtual.Id))
                    _contexto.Secoes.Remove(secaoAtual);
            }

            foreach (var secao in secoes)
                Editar(secao);
        }

        public void Editar(Secao secao)
        {
            var old = _contexto.Secoes.FirstOrDefault(x => x.Id == secao.Id);

            if (old != null)
            {
                _contexto.Entry(old).CurrentValues.SetValues(secao);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
