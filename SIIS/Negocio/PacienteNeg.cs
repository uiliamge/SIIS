using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;

namespace SIIS.Negocio
{
    public class PacienteNeg : IDisposable
    {
        readonly SiteDataContext _contexto = new SiteDataContext();

        public void Inserir(ApplicationUser user, RegisterPacienteViewModel model)
        {
            var paciente = new Paciente
            {
                Nome = user.NomeCompleto,
                Email = user.Email,
                CpfCnpj = user.Cpf,
                DataHora = DateTime.Now,
                UserId = user.Id,
                Cep = model.Cep,
                Endereco = model.Endereco,
                Uf = model.Uf,
                Cidade = model.Cidade,
                Ip = user.Ip
            };
            _contexto.Pacientes.Add(paciente);
        }

        public void Editar(Paciente paciente)
        {
            var old = _contexto.Pacientes.FirstOrDefault(x => x.Id == paciente.Id);

            if (old != null)
            {
                _contexto.Entry(old).CurrentValues.SetValues(paciente);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public void SalvarPermissoesPaciente(string userId, List<PermissaoPacienteViewModel> lstPermissaoPaciente)
        {
            List<PermissaoResponsavelPaciente> permissoesAtuais = _contexto.PermissoesResponsavelPaciente.ToList();
            List<PermissaoResponsavelPaciente> permissoesNovas =
                lstPermissaoPaciente.Select(permissaoNova => new PermissaoResponsavelPaciente
                {
                    Paciente = _contexto.Pacientes.FirstOrDefault(x => x.UserId == userId),
                    NumeroConselho = permissaoNova.NumeroConselho,
                    SiglaConselhoRegional = permissaoNova.SiglaConselhoRegional,
                    UfConselhoRegional = permissaoNova.UfConselhoRegional
                }).ToList();

            //Adiciona novos
            foreach (var permissaoNova in permissoesNovas)
            {
                if (!permissoesAtuais.Contains(permissaoNova))
                {
                    _contexto.PermissoesResponsavelPaciente.Add(permissaoNova);
                }
            }

            //Remove os excluídos da lista
            foreach (var permissaoAtual in permissoesAtuais)
            {
                if (!permissoesNovas.Contains(permissaoAtual))
                {
                    _contexto.PermissoesResponsavelPaciente.Remove(permissaoAtual);
                }
            }            
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
