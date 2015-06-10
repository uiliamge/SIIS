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
        
        public ActionResult ListarExtratos(int idResponsavel, string codigo, string cpf,
            string datainicio, string dataFim, string nome, string cidade, string plano, string orderBy, int? page)
        {
            IPagedList<Extrato> extratos;

            var tipoOrderAnterior = TempData["tipoOrderByAnterior"] as string;
            var orderByAnterior =   TempData["orderByAnterior"] as string;

            var tipoOrderBy = orderBy == orderByAnterior 
                ? (tipoOrderAnterior == "asc" ? "desc" : "asc")
                : "asc";

            TempData["tipoOrderByAnterior"] = tipoOrderBy;
            TempData["orderByAnterior"] = orderBy;

            using (ExtratoNeg extratoNeg = new ExtratoNeg())
            {
                extratos = extratoNeg.Listar(idResponsavel, codigo, cpf, datainicio, dataFim, nome, cidade, plano, orderBy, tipoOrderBy, page);
            }

            return PartialView("_ExtratosResponsavel", extratos);
        }
    }
}
