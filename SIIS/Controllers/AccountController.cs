using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SIIS.Helpers;
using SIIS.Models;
using SIIS.Negocio;
using Microsoft.AspNet.Identity;

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

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();

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
                    if (!user.EmailConfirmed)
                    {
                        Danger("O seu cadastro ainda não foi validado. Por favor, confira a sua caixa de e-mail \"" +
                               user.Email + "\".", true);
                    }

                    await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);                    
                }
                else
                {
                    ModelState.AddModelError("", "Login inválido.");
                }
            }
            // If we got this far, something failed, redisplay form
            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();

            return View(model);
        }

        #region Profissional Register e Editar

        [AllowAnonymous]
        public ActionResult RegisterProfissional()
        {
            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagUf();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterProfissional(RegisterProfissionalViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.userName,
                        NomeCompleto = model.NomeCompleto,
                        Email = model.Email,
                        NumeroConselho = model.NumeroConselho,
                        SiglaConselhoRegional = model.SiglaConselhoRegional.ToString(),
                        UfConselhoRegional = model.UfConselhoRegional,
                        TipoUsuario = TipoUsuarioEnum.Profissional,
                        Ip = Request.UserHostAddress
                    };

                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        using (ResponsavelNeg profissionalNeg = new ResponsavelNeg())
                        {
                            profissionalNeg.Inserir(user, model);
                        }

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                            protocol: Request.Url.Scheme);

                        string dominioAlterado = string.Join(null, Regex.Split(callbackUrl.Split('/')[2], "[\\d]"));
                        dominioAlterado = dominioAlterado.Remove(dominioAlterado.Length - 1) + "/";

                        string strCallbackAlterada = callbackUrl.Split('/')[0] + "//" + dominioAlterado +
                                                     callbackUrl.Split('/')[3] + "/" + callbackUrl.Split('/')[4];

                        string hrefCallback = dominioAlterado == "localhost/" ? callbackUrl : strCallbackAlterada;

                        string mensagem = "<h2>Obrigado por se cadastrar no SIIS</h2>" +
                                          "<h3>Sistema de Integração de Informações de Saúde</h3>" +
                                          "<p>Por favor, confirme a sua conta clicando <a href=\"" + hrefCallback +
                                          "\">aqui</a>.</p>";

                        await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mensagem);

                        Success(
                            "Obrigado! Confira a sua caixa de e-mail \"" + model.Email + "\" para validar o seu acesso.",
                            true);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Danger(String.Join("\n", result.Errors.ToList()), true);
                        AddErrors(result);
                    }
                }

                catch (DbEntityValidationException e)
                {
                    var dbEntityValidationResults = e.EntityValidationErrors as List<DbEntityValidationResult>;
                    if (dbEntityValidationResults != null)
                        foreach (var erro in dbEntityValidationResults)
                        {
                            Danger(String.Join("\n", erro.ValidationErrors.Select(x => x.ErrorMessage)), true);
                        }
                }
                catch (DbUpdateException e)
                {
                    Danger(e.InnerException.ToString(), true);
                }
                catch (Exception e)
                {
                    Danger(e.Message, true);
                }
            }
            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagUf();
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Paciente Register e Editar
        [AllowAnonymous]
        public ActionResult RegisterPaciente()
        {
            List<SelectListItem> profissionaisListItens = new List<SelectListItem>();
            //foreach
            ViewBag.lbxProfissionaisSelecionados = profissionaisListItens;

            RegisterPacienteViewModel model = new RegisterPacienteViewModel
            {
                Permissao = new List<PermissaoPacienteViewModel>()
            };

            TempData["ProfissionaisPermitidos"] = null;

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagTipoPermissao();
            CarregarViewBagUf();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterPaciente(RegisterPacienteViewModel model)
        {
            try
            {
                model.Permissao = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel> ??
                                  new List<PermissaoPacienteViewModel>();

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.userName,
                        NomeCompleto = model.NomeCompleto,
                        Email = model.Email,
                        Cpf = model.Cpf.RemoverMascara(),
                        TipoUsuario = TipoUsuarioEnum.Paciente,
                        Ip = Request.UserHostAddress
                    };

                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        using (PacienteNeg pacienteNeg = new PacienteNeg())
                        {
                            pacienteNeg.Inserir(user, model);
                        }
                        using (PacienteNeg pacienteNeg = new PacienteNeg())
                        {
                            pacienteNeg.SalvarPermissoesPaciente(user.Id, model.Permissao);
                        }
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                            protocol: Request.Url.Scheme);

                        string dominioAlterado = string.Join(null, Regex.Split(callbackUrl.Split('/')[2], "[\\d]"));
                        dominioAlterado = dominioAlterado.Remove(dominioAlterado.Length - 1) + "/";

                        string strCallbackAlterada = callbackUrl.Split('/')[0] + "//" + dominioAlterado +
                                                     callbackUrl.Split('/')[3] + "/" + callbackUrl.Split('/')[4];

                        string hrefCallback = dominioAlterado == "localhost/" ? callbackUrl : strCallbackAlterada;

                        string mensagem = "<h2>Obrigado por se cadastrar no SIIS</h2>" +
                                          "<h3>Sistema de Integração de Informações de Saúde</h3>" +
                                          "<p>Por favor, confirme a sua conta clicando <a href=\"" + hrefCallback +
                                          "\">aqui</a>.</p>";

                        await UserManager.SendEmailAsync(user.Id, "Confirme sua Conta", mensagem);

                        Success(
                            "Obrigado! Confira a sua caixa de e-mail \"" + model.Email + "\" para validar o seu acesso.",
                            true);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Danger(String.Join("\n", result.Errors.ToList()), true);
                        AddErrors(result);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                var dbEntityValidationResults = e.EntityValidationErrors as List<DbEntityValidationResult>;
                if (dbEntityValidationResults != null)
                    foreach (var erro in dbEntityValidationResults)
                    {
                        Danger(String.Join("\n", erro.ValidationErrors.Select(x => x.ErrorMessage)), true);
                    }
            }
            catch (DbUpdateException e)
            {
                Danger(e.InnerException.ToString(), true);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagTipoPermissao();
            CarregarViewBagUf();

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarPaciente(Paciente model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserManager.FindById(model.UserId);

                    user.NomeCompleto = model.Nome;
                    user.Email = model.Email;
                    user.Cpf = model.CpfCnpj;
                    user.Ip = Request.UserHostAddress;
                    model.Ip = Request.UserHostAddress;

                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        using (PacienteNeg pacienteNeg = new PacienteNeg())
                        {
                            pacienteNeg.Editar(model);
                        }

                        Success("Dados cadastrais alterados com sucesso.", true);

                        //return RedirectToAction("Manage", new { Message = ManageMessageId.DadosCadastraisAlterados });
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        Danger(String.Join("\n", result.Errors.ToList()), true);
                        AddErrors(result);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                Danger(String.Join("\n", e.EntityValidationErrors.ToList()), true);
            }
            catch (DbUpdateException e)
            {
                Danger(e.InnerException.ToString(), true);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagTipoPermissao(model);
            CarregarViewBagUf(model);

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarResponsavel(Responsavel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = UserManager.FindById(model.UserId);

                    user.NomeCompleto = model.Nome;
                    user.Email = model.Email;
                    user.Cpf = model.CpfCnpj;
                    user.Ip = Request.UserHostAddress;
                    model.Ip = Request.UserHostAddress;
                    user.NumeroConselho = model.NumeroConselhoRegional;
                    user.SiglaConselhoRegional = model.SiglaConselhoRegional.ToString();
                    user.UfConselhoRegional = model.UfConselhoRegional;

                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        using (ResponsavelNeg responsavelNeg = new ResponsavelNeg())
                        {
                            responsavelNeg.Editar(model);
                        }

                        Success("Dados cadastrais alterados com sucesso.", true);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        Danger(String.Join("\n", result.Errors.ToList()), true);
                        AddErrors(result);
                    }
                }
                else
                {
                    Danger("O Cadastro não foi atualizado.", true);
                }
            }
            catch (DbEntityValidationException e)
            {
                Danger(String.Join("\n", e.EntityValidationErrors.ToList()), true);
            }
            catch (DbUpdateException e)
            {
                Danger(e.InnerException.ToString(), true);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagUf(null, model);

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarPermissoesPaciente(Paciente paciente)
        {
            try
            {
                var permissoes = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel> ??
                                  new List<PermissaoPacienteViewModel>();

                using (PacienteNeg pacienteNeg = new PacienteNeg())
                {
                    pacienteNeg.AlterarTipoPermissao(paciente);
                    pacienteNeg.SalvarPermissoesPaciente(paciente.Id, permissoes);
                }

                Success("Dados de permissão alterados com sucesso.", true);

                return RedirectToAction("Manage");

            }
            catch (DbEntityValidationException e)
            {
                Danger(String.Join("\n", e.EntityValidationErrors.ToList()), true);
            }
            catch (DbUpdateException e)
            {
                Danger(e.InnerException.ToString(), true);
            }
            catch (Exception e)
            {
                Danger(e.Message, true);
            }

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagTipoPermissao(paciente);
            CarregarViewBagUf();

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Manage");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddTempDataProfissionaisPermitidos(string numero, int sigla, int uf, bool edicao)
        {
            var permitidosAtuais = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel> ??
                                   new List<PermissaoPacienteViewModel>();

            var nova = new PermissaoPacienteViewModel
            {
                NumeroConselho = Convert.ToInt32(numero),
                SiglaConselhoRegional = (ConselhoEnum)sigla,
                UfConselhoRegional = (UfEnum)uf
            };

            if (!permitidosAtuais.Contains(nova))
                permitidosAtuais.Add(nova);

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagTipoPermissao();

            TempData["ProfissionaisPermitidos"] = permitidosAtuais;

            if (edicao)
                return PartialView("_PermissoesPaciente", permitidosAtuais);

            return PartialView("_RegisterPacienteProfissionaisPermitidos", permitidosAtuais);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveTempDataProfissionaisPermitidos(string numero, int sigla, int uf, bool edicao)
        {
            var permitidosAtuais = TempData["ProfissionaisPermitidos"] as List<PermissaoPacienteViewModel> ??
                                   new List<PermissaoPacienteViewModel>();

            var remover = new PermissaoPacienteViewModel
            {
                NumeroConselho = Convert.ToInt32(numero),
                SiglaConselhoRegional = (ConselhoEnum)sigla,
                UfConselhoRegional = (UfEnum)uf
            };

            permitidosAtuais = permitidosAtuais.Where(x => x.NumeroConselho != remover.NumeroConselho).ToList();

            CarregarViewBagSiglaConselhoRegional();
            CarregarViewBagUfConselhoRegional();
            CarregarViewBagTipoPermissao();
            CarregarViewBagUf();

            TempData["ProfissionaisPermitidos"] = permitidosAtuais;

            if (edicao)
                return PartialView("_PermissoesPaciente", permitidosAtuais);

            return PartialView("_RegisterPacienteProfissionaisPermitidos", permitidosAtuais);
        }

        #endregion

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

        #region Password

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

        #endregion

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

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Sua senha foi alterada com sucesso."
                : message == ManageMessageId.DadosCadastraisAlterados ? "Dados cadastrais alterados com sucesso."
                : message == ManageMessageId.PermissoesAlteradas ? "Dados de permissão alterados com sucesso."
                : message == ManageMessageId.Error ? "Ocorreu um erro."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");

            var username = User.Identity.GetUserName();
            ApplicationUser user = ApplicationUser.GetUsuario(username);

            if (user.TipoUsuario == TipoUsuarioEnum.Paciente)
            {
                Paciente paciente = Paciente.GetByUserId(User.Identity.GetUserId());
                paciente.CpfCnpj = paciente.CpfCnpj;

                CarregarViewBagUf(paciente);
                CarregarViewBagTipoPermissao(paciente);
                CarregarViewBagSiglaConselhoRegional();
                CarregarViewBagUfConselhoRegional();
            }
            else
            {
                Responsavel responsavel = Responsavel.GetByUserId(User.Identity.GetUserId());
                responsavel.CpfCnpj = responsavel.CpfCnpj;

                CarregarViewBagUf(null, responsavel);
                CarregarViewBagSiglaConselhoRegional(responsavel);
                CarregarViewBagUfConselhoRegional(responsavel);
            }

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
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        Success("Sua senha foi alterada com sucesso.", true);
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

        #region ViewBags

        private void CarregarViewBagTipoPermissao(Paciente paciente = null)
        {
            List<SelectListItem> tipoPermissaoListItens = new List<SelectListItem>();

            foreach (var tipoPermissao in Enum.GetValues(typeof(TipoPermissaoEnum)))
            {
                FieldInfo fi = tipoPermissao.GetType().GetField(tipoPermissao.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string description = attributes[0].Description;

                tipoPermissaoListItens.Add(new SelectListItem
                {
                    Text = description,
                    Value = tipoPermissao.ToString(),
                    Selected = (paciente != null && paciente.TipoPermissao == (TipoPermissaoEnum)tipoPermissao)
                });
            }

            ViewBag.TipoPermissao = tipoPermissaoListItens;
        }
        private void CarregarViewBagUf(Paciente paciente = null, Responsavel responsavel = null)
        {
            #region UfConselho

            List<SelectListItem> ufListItens = new List<SelectListItem>();

            if (paciente == null && responsavel == null)
                ufListItens.Add(new SelectListItem { Selected = true, Text = "", Value = "" });

            ufListItens.AddRange(from object uf in Enum.GetValues(typeof(UfEnum))
                                 select new SelectListItem
                                 {
                                     Text = uf.ToString(),
                                     Value = ((int)uf).ToString(),
                                     Selected = (paciente != null && paciente.Uf == (UfEnum)uf) || (responsavel != null && responsavel.Uf == (UfEnum)uf)
                                 });

            ViewBag.Uf = ufListItens;

            #endregion
        }
        private void CarregarViewBagUfConselhoRegional(Responsavel responsavel = null)
        {
            List<SelectListItem> ufListItens = new List<SelectListItem>();

            if (responsavel == null)
                ufListItens.Add(new SelectListItem { Selected = true, Text = "", Value = "" });

            ufListItens.AddRange(from object uf in Enum.GetValues(typeof(UfEnum))
                                 select new SelectListItem
                                 {
                                     Text = uf.ToString(),
                                     Value = ((int)uf).ToString(),
                                     Selected = (responsavel != null && responsavel.Uf == (UfEnum)uf)
                                 });

            ViewBag.UfConselhoRegional = ufListItens;
        }
        private void CarregarViewBagSiglaConselhoRegional(Responsavel responsavel = null)
        {
            List<SelectListItem> conselhosListItens = new List<SelectListItem>();
            if (responsavel == null)
                conselhosListItens.Add(new SelectListItem { Selected = true, Text = "", Value = "" });

            foreach (var conselho in Enum.GetValues(typeof(ConselhoEnum)))
            {
                conselhosListItens.Add(new SelectListItem
                {
                    Text = conselho.ToString(),
                    Value = ((int)conselho).ToString(),
                    Selected = (responsavel != null && responsavel.SiglaConselhoRegional == (ConselhoEnum)conselho)
                });
            }

            ViewBag.SiglaConselhoRegional = conselhosListItens;
        }

        #endregion

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
            Error,
            DadosCadastraisAlterados,
            PermissoesAlteradas
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
    }
}