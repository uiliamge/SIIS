using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SIIS.Models;

namespace SIIS.Controllers
{
    public class PacienteController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly SiteDataContext _context = new SiteDataContext();

        public PacienteController()
        {
        }

        public PacienteController(ApplicationUserManager userManager)
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


    }
}
