using SIIS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SIIS.Negocio;
using PagedList;

namespace SIIS.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "Início";
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }
        
        public ActionResult FiltrarExtratosResponsavel(int idResponsavel, string codigo, string cpf,
            string datainicio, string dataFim, string nome, string cidade, string orderBy, int? page)
        {
            IPagedList<Extrato> extratos;

            using (ExtratoNeg extratoNeg = new ExtratoNeg())
            {
                extratos = extratoNeg.Listar(idResponsavel, codigo, cpf, datainicio, dataFim, nome, cidade, orderBy, page);
            }

            return PartialView("_ExtratosResponsavel", extratos);
        }


    }
}
