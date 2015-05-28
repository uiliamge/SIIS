using System.Web.Mvc;

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
        public ActionResult Login()
        {
            return View();
        }
    }
}
