using System;
using System.Web.Mvc;
using SIIS.Models;

namespace SIIS.Controllers
{
    public class ExtratoController : Controller
    {
        //
        // GET: /Extrato/

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
            return View();
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
    }
}
