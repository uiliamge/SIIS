using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;

namespace SIIS.Negocio
{
    public class ComposicaoNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();
        
        public ComposicaoNeg()
        {
            _contexto = new SiteDataContext();
        }

        public ComposicaoNeg(SiteDataContext contexto)
        {
            _contexto = contexto;
        }

        public void Editar(IEnumerable<Composicao> composicoes)
        {
            foreach (var composicao in composicoes)
            {
                Editar(composicao);                
            }
        }

        public void Editar(Composicao composicao)
        {
            SecaoNeg secaoNeg = new SecaoNeg(_contexto);
            secaoNeg.Editar(composicao.Secoes);

            var old = _contexto.Composicoes.FirstOrDefault(x => x.Id == composicao.Id);

            if (old != null)
            {
                _contexto.Entry(old).CurrentValues.SetValues(composicao);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
