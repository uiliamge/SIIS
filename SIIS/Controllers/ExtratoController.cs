using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SIIS.Models;
using SIIS.Negocio;
using System.Linq;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace SIIS.Controllers
{
    [Authorize]
    public class ExtratoController : BaseController
    {
        private ApplicationUserManager _userManager;

        public ExtratoController() { }

        public ExtratoController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Importar ou digitar Extratos de Prontuários";
            return View();
        }

        public ActionResult Consulta()
        {
            return View();
        }

        public ActionResult Importar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Importar(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Preencher(int? id)
        {
            Extrato extrato;

            if (id == null || id == 0)
            {
                string userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var responsavel = Responsavel.GetByUserId(userId);

                extrato = new Extrato
                {
                    DataReferencia = DateTime.Now,
                    Responsavel = responsavel,
                    IndImportado = 0,
                    Ip = Request.UserHostAddress
                };
            }
            else
            {
                using (ExtratoNeg extratoNeg = new ExtratoNeg())
                {
                    extrato = extratoNeg.Buscar(id ?? 0);
                }
            }

            return View(extrato);
        }

        [HttpPost]
        public ActionResult Preencher(Extrato extrato, bool? addComposicao, bool? addSecao,
            int? composicaoParaDeletar, int? secaoParaDeletar, int? composicaoDaSecao)
        {
            try
            {
                //Nova composição
                if (addComposicao == true)
                {
                    var novaComposicao = new Composicao
                    {
                        Indice = extrato.Composicoes.OrderByDescending(x => x.Indice).Select(x => x.Indice).First() + 1
                    };
                    novaComposicao.Secoes.Add(new Secao { Indice = 0 });
                    extrato.Composicoes.Add(novaComposicao);
                }
                //Nova seção
                if (addSecao == true)
                {
                    var composicao = extrato.Composicoes.First(x => x.Id == composicaoDaSecao);
                    extrato.Composicoes.First(x => x.Id == composicaoDaSecao).Secoes.Add(new Secao
                    {
                        Indice = composicao.Secoes.OrderByDescending(x => x.Indice).Select(x => x.Indice).First() + 1
                    });
                }
                //Deletar composição
                if (composicaoParaDeletar != null)
                {
                    if (extrato.Composicoes.Count == 1)
                        throw new InvalidOperationException("O Extrato deve possuir ao menos uma Composição.");

                    extrato.Composicoes.Remove(extrato.Composicoes.FirstOrDefault(x => x.Id == composicaoParaDeletar));
                }

                //Deletar seção
                if (secaoParaDeletar != null)
                {
                    Secao secaoRemove = new Secao();
                    foreach (var composicao in extrato.Composicoes)
                    {
                        secaoRemove = composicao.Secoes.FirstOrDefault(secao => secao.Id == secaoParaDeletar);
                        secaoRemove.Composicao = composicao;
                    }
                    if (extrato.Composicoes.FirstOrDefault(x => x.Id == secaoRemove.Composicao.Id).Secoes.Count == 1)
                        throw new InvalidOperationException("A Composição deve possuir ao menos uma Seção.");

                    extrato.Composicoes.FirstOrDefault(x => x.Id == secaoRemove.Composicao.Id)
                        .Secoes.Remove(secaoRemove);
                }

                SalvarExtrato(extrato);
                if (addComposicao != true && addSecao != true && composicaoParaDeletar != null && secaoParaDeletar != null)
                    Success("Extrato salvo com sucesso.", true);
                else
                    Success("Extrato alterado com sucesso.", true);
            }
            catch (InvalidOperationException e)
            {
                Warning(e.Message, true);
            }
            catch (DbEntityValidationException e)
            {
                var dbEntityValidationResults = e.EntityValidationErrors as List<DbEntityValidationResult>;
                if (dbEntityValidationResults != null)
                    foreach (var erro in dbEntityValidationResults)
                    {
                        Danger(String.Join("\n", erro.ValidationErrors.Select(x => x.ErrorMessage)), true);
                    }
            }
            catch (DbUpdateException e)
            {
                Danger(e.InnerException.ToString(), true);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }

            return RedirectToAction("Preencher", new { id = extrato.Id });
        }

        private void SalvarExtrato(Extrato extrato)
        {
            using (ExtratoNeg extratoNeg = new ExtratoNeg())
            {
                extrato.Ip = Request.UserHostAddress;

                if (extrato.Id == 0)
                {
                    Secao secao = new Secao { Indice = 0 };
                    Composicao composicao = new Composicao { Indice = 0 };
                    composicao.Secoes.Add(secao);
                    extrato.Composicoes.Add(composicao);

                    extratoNeg.Inserir(extrato);
                }
                else
                {
                    extratoNeg.Editar(extrato);
                }
            }
        }

        [HttpPost]
        public ActionResult VerificarSalvar(Extrato extrato)
        {
            try
            {
                SalvarExtrato(extrato);
            }
            catch (DbEntityValidationException e)
            {
                var dbEntityValidationResults = e.EntityValidationErrors as List<DbEntityValidationResult>;
                if (dbEntityValidationResults != null)
                    foreach (var erro in dbEntityValidationResults)
                    {
                        Danger(String.Join("\n", erro.ValidationErrors.Select(x => x.ErrorMessage)), true);
                    }
            }
            catch (DbUpdateException e)
            {
                Danger(e.InnerException.ToString(), true);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }

            return RedirectToAction("Preencher", new { id = extrato.Id });
        }

        [HttpPost]
        public ActionResult BuscarPaciente(string cpf)
        {
            Paciente paciente = new Paciente();
            try
            {
                using (PacienteNeg pacienteNeg = new PacienteNeg())
                {
                    paciente = pacienteNeg.BuscarPorCpf(cpf);
                }

                return PartialView("_DadosCadastraisPaciente", paciente);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }
            return PartialView("_DadosCadastraisPaciente", new Paciente());
        }

        public ActionResult ListarExtratos(string codigo, string cpf, string datainicio, string dataFim,
            string cidade, string responsavel, string orderBy, int? page)
        {
            IPagedList<Extrato> extratos = new PagedList<Extrato>(new List<Extrato>(), 1, 10);

            if (!string.IsNullOrEmpty(cpf))
            {
                var tipoOrderAnterior = TempData["tipoOrderByAnterior"] as string;
                var orderByAnterior = TempData["orderByAnterior"] as string;

                var tipoOrderBy = orderBy == orderByAnterior
                    ? (tipoOrderAnterior == "asc" ? "desc" : "asc")
                    : "asc";

                TempData["tipoOrderByAnterior"] = tipoOrderBy;
                TempData["orderByAnterior"] = orderBy;

                using (ExtratoNeg extratoNeg = new ExtratoNeg())
                {
                    extratos = extratoNeg.Listar(codigo, cpf, datainicio, dataFim, cidade, responsavel, orderBy,
                        tipoOrderBy, page);
                }
            }
            return PartialView("_ExtratosConsulta", extratos);
        }

        public ActionResult AbrirModalExtrato(int id)
        {            
            var extrato = new Extrato();
            try
            {
                using (ExtratoNeg extratoNeg = new ExtratoNeg())
                {
                    extrato = extratoNeg.Buscar(id);
                }                
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }
            return PartialView("_ConsultaExtratoModal", extrato);
        }
    }
}
