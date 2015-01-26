using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAppMvcJabu.Models;
using System.Collections.Generic;

namespace WebAppMvcJabu.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie,
                                          DefaultAuthenticationTypes.ApplicationCookie,
                                          DefaultAuthenticationTypes.TwoFactorCookie,
                                          DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie,
                                          DefaultAuthenticationTypes.ExternalBearer);

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    Utils.WriteLogMessage(model.Email, "El usuario inicio sesión en la aplicación.");
                    return RedirectToLocal("/Dashboard/Index");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
            // Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
            // se bloqueará durante un período de tiempo especificado. 
            // Puede configurar el bloqueo de la cuenta en IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código no válido.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Titulo = "Registra Usuario";
            ViewBag.Disabled = false;

            var vm = new RegisterViewModel { RolesList = new SelectList(RoleManager.Roles.Where(u => u.Name == "Admin").ToList(), "Name", "Name") };
            return PartialView("_PartialRegister", vm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == null || model.Id == "")
                {
                    #region Registra Usuario

                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (selectedRoles != null)
                        {
                            var resultRoles = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                            if (resultRoles.Succeeded)
                            {
                                ModelState.Clear();

                                ViewBag.Titulo = "Registra Usuario";
                                ViewBag.Disabled = false;
                                model = new RegisterViewModel();
                                model.RolesList = new SelectList(RoleManager.Roles.Where(u => u.Name == "Admin").ToList(), "Name", "Name");
                                return PartialView("_PartialRegister", model);
                            }
                            AddErrors(resultRoles);
                        }
                        else
                        {
                            var resultRoles = await UserManager.AddToRoleAsync(user.Id, "User");
                            if (resultRoles.Succeeded)
                            {
                                ModelState.Clear();

                                ViewBag.Titulo = "Registra Usuario";
                                ViewBag.Disabled = false;
                                model = new RegisterViewModel();
                                model.RolesList = new SelectList(RoleManager.Roles.Where(u => u.Name == "Admin").ToList(), "Name", "Name");
                                return PartialView("_PartialRegister", model);
                            }
                            AddErrors(resultRoles);
                        }
                    }
                    AddErrors(result);

                    #endregion
                }
                else
                {
                    #region Edita Usuario

                    var user = await UserManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        await UserManager.RemovePasswordAsync(user.Id);
                        await UserManager.AddPasswordAsync(user.Id, model.Password);

                        var userRoles = await UserManager.GetRolesAsync(user.Id);

                        selectedRoles = selectedRoles ?? new string[] { "User" };

                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles.Except(userRoles).ToArray<string>());
                        if (result.Succeeded)
                        {
                            result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRoles).ToArray<string>());
                            if (result.Succeeded)
                            {
                                ModelState.Clear();

                                ViewBag.Titulo = "Registra Usuario";
                                ViewBag.Disabled = false;
                                model = new RegisterViewModel();
                                model.RolesList = new SelectList(RoleManager.Roles.Where(u => u.Name == "Admin").ToList(), "Name", "Name");
                                return PartialView("_PartialRegister", model);
                            }
                            AddErrors(result);
                        }
                        AddErrors(result);
                    }

                    #endregion
                }
            }

            ViewBag.Titulo = "Registra Usuario";
            ViewBag.Disabled = false;
            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            model.RolesList = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            return PartialView("_PartialRegister", model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> EditUser(string id)
        {
            ViewBag.Titulo = "Edita Usuario";
            ViewBag.Disabled = true;

            if (id != null && id != "")
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var userRoles = await UserManager.GetRolesAsync(user.Id);

                    return PartialView("_PartialRegister", new RegisterViewModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        RolesList = RoleManager.Roles.ToList().Where(u => u.Name == "Admin").Select(x => new SelectListItem()
                        {
                            Selected = userRoles.Contains(x.Name),
                            Text = x.Name,
                            Value = x.Name
                        })
                    });
                }
            }

            var vm = new RegisterViewModel { RolesList = new SelectList(RoleManager.Roles.ToList(), "Name", "Name") };
            return PartialView("_PartialRegister", vm);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var vm = new List<RegisterViewModel>();

            if (ModelState.IsValid)
            {
                if (id != null)
                {
                    var user = await UserManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        var result = await UserManager.DeleteAsync(user);
                        if (result.Succeeded)
                        {
                            foreach (var usuario in UserManager.Users.ToList())
                            {
                                var userRoles = UserManager.GetRoles(usuario.Id);

                                var rvm = new RegisterViewModel();
                                rvm.Id = usuario.Id;
                                rvm.Email = usuario.UserName;
                                rvm.RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                                {
                                    Selected = userRoles.Contains(x.Name),
                                    Text = x.Name,
                                    Value = x.Name
                                });

                                vm.Add(rvm);
                            }

                            return PartialView("_PartialAdminUsuarios", vm.OrderBy(u => u.Email).ToList());
                        }
                    }
                }
            }
            return PartialView("_PartialAdminUsuarios", vm.OrderBy(u => u.Email).ToList());
        }

        [AllowAnonymous]
        public ActionResult GetUsersAndRoles()
        {
            var vm = new List<RegisterViewModel>();

            foreach (var user in UserManager.Users.ToList())
            {
                var userRoles = UserManager.GetRoles(user.Id);

                var rvm = new RegisterViewModel();
                rvm.Id = user.Id;
                rvm.Email = user.UserName;
                rvm.RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                });

                vm.Add(rvm);
            }

            return PartialView("_PartialAdminUsuarios", vm.OrderBy(u => u.Email).ToList());
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

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
                    // No revelar que el usuario no existe o que no está confirmado
                    return View("ForgotPasswordConfirmation");
                }

                // Para obtener más información sobre cómo habilitar la confirmación de cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
                // Enviar correo electrónico con este vínculo
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Solicitar redireccionamiento al proveedor de inicio de sesión externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generar el token y enviarlo
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Si el usuario no tiene ninguna cuenta, solicitar que cree una
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Obtener datos del usuario del proveedor de inicio de sesión externo
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
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