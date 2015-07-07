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
                ViewBag.SomatorioValorCobrado = extratoNeg.SomarValorCobrado(idResponsavel, codigo, cpf, datainicio, dataFim, nome, cidade, plano, orderBy, tipoOrderBy, page);
            }

            

            return PartialView("_ExtratosResponsavel", extratos);
        }

        public ActionResult ListarExtratosPaciente(int idPaciente, string codigo, string responsavel,
            string datainicio, string dataFim, string cidade, string plano, string orderBy, int? page)
        {
            IPagedList<Extrato> extratos;

            var tipoOrderAnterior = TempData["tipoOrderByAnterior"] as string;
            var orderByAnterior = TempData["orderByAnterior"] as string;

            var tipoOrderBy = orderBy == orderByAnterior
                ? (tipoOrderAnterior == "asc" ? "desc" : "asc")
                : "asc";

            TempData["tipoOrderByAnterior"] = tipoOrderBy;
            TempData["orderByAnterior"] = orderBy;

            using (ExtratoNeg extratoNeg = new ExtratoNeg())
            {
                extratos = extratoNeg.ListarPorPaciente(idPaciente, codigo, responsavel, datainicio, dataFim, cidade, plano, orderBy, tipoOrderBy, page);
            }

            return PartialView("_ExtratosPaciente", extratos);
        }

        public ActionResult ListarExtratosPorAprovacao(int idPaciente, string orderBy, int? page)
        {
            IPagedList<Extrato> extratos;

            var tipoOrderAnterior = TempData["tipoOrderByAnterior"] as string;
            var orderByAnterior = TempData["orderByAnterior"] as string;

            var tipoOrderBy = orderBy == orderByAnterior
                ? (tipoOrderAnterior == "asc" ? "desc" : "asc")
                : "asc";

            TempData["tipoOrderByAnterior"] = tipoOrderBy;
            TempData["orderByAnterior"] = orderBy;

            using (ExtratoNeg extratoNeg = new ExtratoNeg())
            {
                extratos = extratoNeg.ListarPorAprovacao(idPaciente, orderBy, tipoOrderBy, page);
            }

            return PartialView("_ExtratosPorAprovacao", extratos);
        }

        public void AprovarExtrato(int id)
        {
            using (ExtratoNeg extratoNeg = new ExtratoNeg())
            {
                extratoNeg.Aprovar(id);
            }
        }

        public ActionResult ListarLogAcessos(string userId)
        {            
            return View(Paciente.GetByUserId(userId));
        }

        public ActionResult _ListarLogAcessos(int idPaciente, int? numeroConselho, string responsavel,
            string datainicio, string dataFim, string orderBy, int? page)
        {
            IPagedList<LogAcessoResponsavel> logs;

            var tipoOrderAnterior = TempData["tipoOrderByAnterior"] as string;
            var orderByAnterior = TempData["orderByAnterior"] as string;

            var tipoOrderBy = orderBy == orderByAnterior
                ? (tipoOrderAnterior == "asc" ? "desc" : "asc")
                : "asc";

            TempData["tipoOrderByAnterior"] = tipoOrderBy;
            TempData["orderByAnterior"] = orderBy;

            using (LogAcessoNeg logNeg = new LogAcessoNeg())
            {
                logs = logNeg.Listar(idPaciente, numeroConselho, responsavel, datainicio, dataFim, orderBy, tipoOrderBy, page);
            }

            return PartialView("_ListarLogAcessos", logs);
        }
    }
}
