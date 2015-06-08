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

        public void Editar(IEnumerable<Composicao> composicoes, int idExtrato)
        {            
            //Composições para excluir
            foreach (var composicaoAtual in _contexto.Composicoes.Where(x => x.Extrato.Id == idExtrato).ToList())
            {
                if (!composicoes.Select(x => x.Id).Contains(composicaoAtual.Id))
                {
                    _contexto.Secoes.RemoveRange(_contexto.Secoes.Where(x => x.Composicao.Id == composicaoAtual.Id));
                    _contexto.Composicoes.Remove(composicaoAtual);
                }
            }

            foreach (var composicao in composicoes)
                Editar(composicao);                
        }

        public void Editar(Composicao composicao)
        {
            SecaoNeg secaoNeg = new SecaoNeg(_contexto);
            secaoNeg.Editar(composicao.Secoes, composicao.Id);

            var novaSecao = composicao.Secoes.FirstOrDefault(x => x.Id == 0);
            if (novaSecao != null)
            {
                novaSecao.Composicao = _contexto.Composicoes.Single(x => x.Id == composicao.Id);
                _contexto.Secoes.Add(novaSecao);
            }

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
