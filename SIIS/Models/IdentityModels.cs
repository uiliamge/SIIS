using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SIIS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public static ApplicationUser UsuarioLogado { get; set; }
        public static ApplicationUser GetUsuario(string userName)
        {
            ApplicationDbContext _contextUsers = new ApplicationDbContext();

            return _contextUsers.Users.FirstOrDefault(x => x.UserName == userName);
        }
        
        public string CssTheme { get; set; }
        
        public string NomeCompleto { get; set; }

        //"paciente" ou "profissional"
        public TipoUsuarioEnum TipoUsuario { get; set; }

        public int NumeroConselho { get; set; }

        public string SiglaConselhoRegional { get; set; }

        public UfEnum UfConselhoRegional { get; set; }

        public string Cpf { get; set; }

        public string Ip { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}