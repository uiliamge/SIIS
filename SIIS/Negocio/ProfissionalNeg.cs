using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;

namespace SIIS.Negocio
{
    public class ProfissionalNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();

        public void Inserir(ApplicationUser user, RegisterProfissionalViewModel model)
        {
            var responsavel = new Responsavel
            {
                Nome = user.NomeCompleto,
                Email = model.Email,
                CpfCnpj = model.CpfCnpj,
                NumeroConselhoRegional = model.NumeroConselho,
                ConselhoRegional = model.SiglaConselhoRegional,
                UfConselhoRegional = model.UfConselhoRegional,
                DataHora = DateTime.Now,
                UserId = user.Id,
                Cep = model.Cep,
                Endereco = model.Endereco,
                NumeroEndereco = model.NumeroEndereco,
                Telefone = model.Telefone,
                Bairro = model.Bairro,
                Complemento = model.Complemento,
                Uf = model.Uf,
                Cidade = model.Cidade,
                Ip = user.Ip
            };
            _contexto.Responsaveis.Add(responsavel);
        }

        public void Editar(Responsavel responsavel)
        {
            responsavel.DataHora = DateTime.Now;
            var old = _contexto.Responsaveis.FirstOrDefault(x => x.Id == responsavel.Id);

            if (old != null)
            {
                _contexto.Entry(old).CurrentValues.SetValues(responsavel);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
