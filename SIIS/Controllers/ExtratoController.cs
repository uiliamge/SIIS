using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SIIS.Models;
using SIIS.Negocio;

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

        public ActionResult Preencher()
        {
            string userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var responsavel = Responsavel.GetByUserId(userId);

            var extrato = new Extrato
            {
                Paciente = new Paciente(),
                DataReferencia = DateTime.Now,
                Responsavel = responsavel,
                IndImportado = 0,
                IP = Request.UserHostAddress
            };

            return View(extrato);
        }

        [HttpPost]
        public ActionResult Preencher(Extrato extrato)
        {
            try
            {
                extrato.DataHora = DateTime.Now;
                extrato.IndImportado = 0;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
    }
}
