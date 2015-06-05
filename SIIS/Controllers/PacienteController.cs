using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SIIS.Models;
using System;
using SIIS.Negocio;

namespace SIIS.Controllers
{
    [Authorize]
    public class PacienteController : BaseController
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
