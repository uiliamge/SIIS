using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using SIIS.Models;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;


namespace SIIS.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly SiteDataContext _context = new SiteDataContext();
        private readonly ApplicationDbContext _contextUsers = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
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

        private void CarregarViewBags()
        {
            #region SiglaConselhoRegional
            
            List<SelectListItem> conselhosListItens = new List<SelectListItem>() { new SelectListItem { Selected = true, Text = "", Value = "" } };
            foreach (var conselho in Enum.GetValues(typeof(ConselhoEnum)))
            {
                conselhosListItens.Add(new SelectListItem { Text = conselho.ToString(), Value = ((int)conselho).ToString() });
            }

            ViewBag.SiglaConselhoRegional = conselhosListItens;

            #endregion

            #region UfConselho

            List<SelectListItem> ufListItens = new List<SelectListItem>() { new SelectListItem { Selected = true, Text = "", Value = "" } };
            foreach (var uf in Enum.GetValues(typeof(UfEnum)))
            {
                ufListItens.Add(new SelectListItem { Text = uf.ToString(), Value = ((int)uf).ToString() });
            }

            ViewBag.UfConselhoRegional = ufListItens;

            #endregion

            #region TipoPermissao

            List<SelectListItem> tipoPermissaoListItens = new List<SelectListItem>();

            foreach (var tipoPermissao in Enum.GetValues(typeof(TipoPermissaoEnum)))
            {
                FieldInfo fi = tipoPermissao.GetType().GetField(tipoPermissao.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = attributes[0].Description;

                tipoPermissaoListItens.Add(new SelectListItem { Text = description, Value = tipoPermissao.ToString() });
            }

            ViewBag.TipoPermissao = tipoPermissaoListItens;

            #endregion
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            CarregarViewBags();
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, int tipoUsuario)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;

                if (tipoUsuario == (int)TipoUsuarioEnum.Profissional)
                    user = await UserManager.FindAsync(model.LoginProfissionalViewModel.UserName, model.Password);
                else
                    user = await UserManager.FindAsync(model.LoginPacienteViewModel.Cpf, model.Password);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        Danger("O seu cadastro ainda não foi validado. Por favor, confira a sua caixa de e-mail \"" +
                            user.Email + "\".", true);
                    }
                }
                else
                {
                    CarregarViewBags();
                    //Danger("Login inválido", true);
                    ModelState.AddModelError("", "Login inválido.");
                }
            }

            // If we got this far, something failed, redisplay form
            CarregarViewBags();
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterProfissional()
        {
            CarregarViewBags();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterProfissional(RegisterProfissionalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.userName,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    NumeroConselho = model.NumeroConselho,
                    SiglaConselhoRegional = model.SiglaConselhoRegional,
                    UfConselhoRegional = model.UfConselhoRegional,
                    TipoUsuario = TipoUsuarioEnum.Profissional
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    string dominioAlterado = string.Join(null, System.Text.RegularExpressions.Regex.Split(callbackUrl.Split('/')[2], "[\\d]"));
                    dominioAlterado = dominioAlterado.Remove(dominioAlterado.Length - 1) + "/";

                    string strCallbackAlterada = callbackUrl.Split('/')[0] + "//" + dominioAlterado + callbackUrl.Split('/')[3] + "/" + callbackUrl.Split('/')[4];

                    string hrefCallback = dominioAlterado == "localhost/" ? callbackUrl : strCallbackAlterada;

                    string mensagem = "<h2>Obrigado por se cadastrar no SIIS</h2>" +
                        "<h3>Sistema de Integração de Informações de Saúde</h3>" +
                        "<p>Por favor, confirme a sua conta clicando <a href=\"" + hrefCallback + "\">aqui</a>.</p>";

                    await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mensagem);

                    Success("Obrigado! Confira a sua caixa de e-mail \"" + model.Email + "\" para validar o seu acesso.", true);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Danger(String.Join("\n", result.Errors.ToList()), true);
                    AddErrors(result);
                }
            }

            CarregarViewBags();
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegisterPaciente()
        {
            List<SelectListItem> profissionaisListItens = new List<SelectListItem>();
            //foreach
            ViewBag.lbxProfissionaisSelecionados = profissionaisListItens;

            RegisterPacienteViewModel model = new RegisterPacienteViewModel();
            model.Permissao = new List<PermissaoPacienteViewModel>();
            CarregarViewBags();

            TempData["ProfissionaisPermitidos"] = null;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterPaciente(RegisterPacienteViewModel model)
        {
            model.Permissao = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel>;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.userName,
                    NomeCompleto = model.NomeCompleto,
                    Email = model.Email,
                    Cpf = model.Cpf,
                    TipoPermissao = model.TipoPermissao,
                    TipoUsuario = TipoUsuarioEnum.Paciente
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    string mensagem = "<h2>Obrigado por se cadastrar no SIIS</h2>" +
                        "<h3>Sistema de Integração de Informações de Saúde</h3>" +
                        "<p>Por favor, confirme a sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>.</p>";

                    await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mensagem);

                    Success("Obrigado! Confira a sua caixa de e-mail \"" + model.Email + "\" para validar o seu acesso.", true);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Danger(String.Join("\n", result.Errors.ToList()), true);
                    AddErrors(result);
                }
            }

            CarregarViewBags();
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddTempDataProfissionaisPermitidos(string numero, int sigla, int uf)
        {
            var permitidosAtuais = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel>;
            
            if (permitidosAtuais == null)
                permitidosAtuais = new List<PermissaoPacienteViewModel>();

            var nova = new PermissaoPacienteViewModel 
            { 
                NumeroConselho = Convert.ToInt32(numero), 
                SiglaConselhoRegional = (ConselhoEnum)sigla, 
                UfConselhoRegional = (UfEnum)uf
            };

            if (!permitidosAtuais.Contains(nova))
                permitidosAtuais.Add(nova);

            CarregarViewBags();

            TempData["ProfissionaisPermitidos"] = permitidosAtuais;

            return PartialView("_RegisterPacienteProfissionaisPermitidos", permitidosAtuais);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveTempDataProfissionaisPermitidos(string numero, int sigla, int uf)
        {            
            var permitidosAtuais = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel>;

            if (permitidosAtuais == null)
                permitidosAtuais = new List<PermissaoPacienteViewModel>();

            var remover = new PermissaoPacienteViewModel
            {
                NumeroConselho = Convert.ToInt32(numero),
                SiglaConselhoRegional = (ConselhoEnum)sigla,
                UfConselhoRegional = (UfEnum)uf
            };

            permitidosAtuais = permitidosAtuais.Where(x => x.NumeroConselho != remover.NumeroConselho).ToList();

            CarregarViewBags();

            TempData["ProfissionaisPermitidos"] = permitidosAtuais;

            return PartialView("_RegisterPacienteProfissionaisPermitidos", permitidosAtuais);
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                Danger(String.Join("\n", result.Errors.ToList()));
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //private IEnumerable<Notification> GetUserNotifications()
        //{
        //    // get the user ID
        //    var userId = User.Identity.GetUserId();

        //    // load our notifications
        //    var notifications = _context.Notifications
        //        .Where(n => n.UserId == userId)
        //        .Where(n => !n.IsDismissed)
        //        .Select(n => n)
        //        .ToList();
        //    return notifications;
        //}

        //[HttpPost]
        //public ActionResult MarkNotificationAsRead(int id)
        //{
        //    var userNotification = GetUserNotifications().FirstOrDefault(n => n.NotificationId == id);

        //    if (userNotification == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }

        //    userNotification.IsDismissed = true;
        //    _context.SaveChanges();

        //    return RedirectToAction("Manage");
        //}

        //public ActionResult GetNotification(int id)
        //{
        //    var userNotification = GetUserNotifications().FirstOrDefault(n => n.NotificationId == id);

        //    if (userNotification == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }

        //    return PartialView("_RenderNotifications.ModalPreview", userNotification);
        //}

        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    var userNotification = GetUserNotifications().FirstOrDefault(n => n.NotificationId == id);

        //    if (userNotification == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }

        //    _context.Notifications.Remove(userNotification);
        //    _context.SaveChanges();

        //    return RedirectToAction("Manage");
        //}

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            //ViewBag.NotificationList = GetUserNotifications();
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            //ViewBag.NotificationList = GetUserNotifications();
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        private Usuario BuscarUsuarioProfissional(LoginViewModel model)
        {
            var numeroConselho = model.LoginProfissionalViewModel.NumeroConselho;
            var siglaConselho = model.LoginProfissionalViewModel.SiglaConselhoRegional;
            var ufConselho = model.LoginProfissionalViewModel.UfConselhoRegional;

            Usuario usuario = _context.Usuarios.FirstOrDefault(x => x.Responsavel.NumeroConselho == numeroConselho
                && x.Responsavel.ConselhoRegional.Sigla == siglaConselho
                && x.Responsavel.UfConselhoRegional == ufConselho);

            return usuario;
        }
    }
}