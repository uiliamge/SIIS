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

            if (id == null)
            {
                string userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var responsavel = Responsavel.GetByUserId(userId);

                extrato = new Extrato
                {
                    Paciente = new Paciente(),
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
            int? composicaoParaDeletar, int? secaoParaDeletar)
        {
            try
            {
                if (addComposicao == true)
                {
                    var novaComposicao = new Composicao
                    {
                        Indice = extrato.Composicoes.OrderByDescending(x => x.Indice).Select(x => x.Indice).First() + 1
                    };
                    novaComposicao.Secoes.Add(new Secao { Indice = 0 });
                    extrato.Composicoes.Add(novaComposicao);
                }

                SalvarExtrato(extrato);
                if(addComposicao != true && addSecao != true)
                    Success("Extrato salvo com sucesso.", true);
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

        #region TempDatas
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddTempDataComposicoes(Composicao composicaoAnte)
        {
            var composicoesAtuais = TempData["ComposicoesPreenchimentoExtrato"] as List<Composicao> ??
                                   new List<Composicao>();

            var nova = new Composicao
            {
                Indice = composicoesAtuais.OrderByDescending(x => x.Indice).Select(x => x.Indice).First() + 1,
                Descricao = ""
            };
            nova.Secoes.Add(new Secao
            {
                Indice = 0,
                IndiceComposicao = nova.Indice
            });

            composicoesAtuais.Add(nova);

            TempData["ComposicoesPreenchimentoExtrato"] = composicoesAtuais;

            return PartialView("_PreencherComposicoes", composicoesAtuais);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveTempDataComposicoes(int indice)
        {
            var composicoesAtuais = TempData["ComposicoesPreenchimentoExtrato"] as List<Composicao> ??
                                   new List<Composicao>();

            var composicoesAtualizadas = composicoesAtuais.Where(x => x.Indice != indice).ToList();

            TempData["ComposicoesPreenchimentoExtrato"] = composicoesAtualizadas;

            return PartialView("_PreencherComposicoes", composicoesAtualizadas);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddTempDataSecoes(int indiceComposicao)
        {
            var secoesAtuais = TempData["SecoesPreenchimentoExtrato"] as List<Secao> ??
                                   new List<Secao>();

            int ultimoIndice = secoesAtuais.OrderByDescending(x => x.Indice).Select(x => x.Indice).First();
            var nova = new Secao
            {
                Indice = ultimoIndice + 1,
                IndiceComposicao = indiceComposicao,
                Nome = ""
            };

            secoesAtuais.Add(nova);

            TempData["SecoesPreenchimentoExtrato"] = secoesAtuais;

            return PartialView("_PreencherComposicoesSecoes", secoesAtuais.Where(x => x.IndiceComposicao == indiceComposicao));
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveTempDataSecoes(int indice, int indiceComposicao)
        {
            var secoesAtuais = TempData["SecoesPreenchimentoExtrato"] as List<Secao> ??
                                   new List<Secao>();

            Secao secaoParaExcluir = secoesAtuais.First(x => x.Indice == indice && x.IndiceComposicao == indiceComposicao);
            secoesAtuais.Remove(secaoParaExcluir);

            TempData["SecoesPreenchimentoExtrato"] = secoesAtuais;

            return PartialView("_PreencherComposicoesSecoes", secoesAtuais);
        }

        #endregion
    }
}
