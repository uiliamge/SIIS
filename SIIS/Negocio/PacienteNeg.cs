using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SIIS.Models;

namespace SIIS.Negocio
{
    public class PacienteNeg : IDisposable
    {
        readonly SiteDataContext _contexto;

        public PacienteNeg()
        {
            _contexto = new SiteDataContext();
        }

        public PacienteNeg(SiteDataContext contexto)
        {
            _contexto = contexto;
        }

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
                NumeroEndereco = model.NumeroEndereco,
                Telefone = model.fone,
                Bairro = model.Bairro,
                Complemento = model.Complemento,
                Uf = model.Uf,
                Cidade = model.Cidade,                
                Ip = user.Ip,

                TipoPermissao = model.TipoPermissao,
                DataNascimento = model.DataNascimento,
            };
            _contexto.Pacientes.Add(paciente);
        }

        public void Editar(Paciente paciente)
        {
            paciente.DataHora = DateTime.Now;
            var old = _contexto.Pacientes.FirstOrDefault(x => x.Id == paciente.Id);

            if (old != null)
            {
                _contexto.Entry(old).CurrentValues.SetValues(paciente);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public Paciente BuscarPorCpf(string cpf)
        {
            return _contexto.Pacientes.FirstOrDefault(x => x.CpfCnpj == cpf);
        }

        public void SalvarPermissoesPaciente(string userId, List<PermissaoPacienteViewModel> lstPermissaoPaciente)
        {
            Paciente paciente = _contexto.Pacientes.FirstOrDefault(x => x.UserId == userId);

            List<PermissaoResponsavelPaciente> permissoesAtuais = _contexto.PermissoesResponsavelPaciente.ToList();
            List<PermissaoResponsavelPaciente> permissoesNovas =
                lstPermissaoPaciente.Select(permissaoNova => new PermissaoResponsavelPaciente
                {
                    Paciente = paciente,
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

        public void SalvarPermissoesPaciente(int idPaciente, List<PermissaoPacienteViewModel> lstPermissaoPaciente)
        {
            Paciente paciente = _contexto.Pacientes.FirstOrDefault(x => x.Id == idPaciente);

            List<PermissaoResponsavelPaciente> permissoesAtuais = _contexto.PermissoesResponsavelPaciente.ToList();
            List<PermissaoResponsavelPaciente> permissoesNovas =
                lstPermissaoPaciente.Select(permissaoNova => new PermissaoResponsavelPaciente
                {
                    Paciente = paciente,
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

        public void AlterarTipoPermissao(Paciente paciente)
        {
            var old = _contexto.Pacientes.FirstOrDefault(x => x.Id == paciente.Id);
            var novo = old;
            novo.TipoPermissao = paciente.TipoPermissao;

            if (old != null)
            {
                _contexto.Entry(old).CurrentValues.SetValues(novo);
                _contexto.Entry(old).State = EntityState.Modified;
            }
        }

        public void Dispose()
        {
            _contexto.SaveChanges();
        }
    }
}
