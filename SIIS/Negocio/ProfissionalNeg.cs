﻿using System;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;

namespace SIIS.Negocio
{
    public class ResponsavelNeg : IDisposable
    {
        readonly SiteDataContext _contexto;

        public ResponsavelNeg()
        {
            _contexto = new SiteDataContext();
        }
        public ResponsavelNeg(SiteDataContext contexto)
        {
            _contexto = contexto;
        }

        public void Inserir(ApplicationUser user, RegisterProfissionalViewModel model)
        {
            var responsavel = new Responsavel
            {
                Nome = user.NomeCompleto,
                Email = user.Email,
                CpfCnpj = model.CpfCnpj,
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
                Ip = user.Ip,

                NumeroConselhoRegional = model.NumeroConselho,
                SiglaConselhoRegional = model.SiglaConselhoRegional,
                UfConselhoRegional = model.UfConselhoRegional,
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

        public Responsavel BuscarPorUserId(string userId)
        {
            return _contexto.Responsaveis.FirstOrDefault(x => x.UserId == userId);
        }

        public Responsavel Buscar(int id)
        {
            return _contexto.Responsaveis.FirstOrDefault(x => x.Id == id);
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
