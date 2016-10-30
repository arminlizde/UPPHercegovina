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
using UPPHercegovina.WebApplication.Models;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.IO;
using System.Net.Mail;
using UPPHercegovina.WebApplication.Helpers;

namespace UPPHercegovina.WebApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        //dodajem nekoliko komentara
        //testiram GIT GIT GIT
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext context = new ApplicationDbContext();

        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        #region nemapotrebe
        //ovo dvoje mi ne treba, za sada, jer ce se koristit u user dijelu
        //malo izmjeniti ime metode i rijesen problem :)
        //updateovat kod, skratit/poboljsati
        public ActionResult Profile()
        {

            ApplicationUser temp = new ApplicationUser();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);



                ViewBag.DisplayName = temp.GetDisplayName();

                if (userIdClaim != null)
                {
                    temp = context.Users.Find(userIdClaim.Value);

                    if (String.IsNullOrWhiteSpace(temp.PictureUrl))
                        temp.PictureUrl = "/Images/UserImg/avatarDefault.png";

                    temp.Id = userIdClaim.Value;
                }
            }
            ViewBag.Township = context.PlaceOfResidences.Find(temp.TownshipId).Name;

            return View(temp);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Profile(
            [Bind(Include = "Id, UserName, FirstName, LastName,AccessFailedCount, PasswordHash, SecurityStamp,IdentificationNumber,PictureUrl,RegistrationDate ,TownshipId, Address, Email, PhoneNumber")]
            ApplicationUser model, FormCollection form, HttpPostedFileBase file)
        {
            ViewBag.Date = DateTime.Now.ToString();
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (!SavePictureToServer(file, model))
                    {
                        ViewBag.message = "Please choose only Image file";
                        return RedirectToAction("Index");
                    }

                }
                context.Entry(model).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Profile", "Account");
            }
            return RedirectToAction("Profile", "Account");

        }
        #endregion

        private bool SavePictureToServer(HttpPostedFileBase file, ApplicationUser model)
        {
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

            var ext = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(ext.ToLower()))
                return false;

            model.PictureUrl = string.Format("/Images/UserImg/Usr-{0}{1}-{2}-{3}-{4}.jpg", model.FirstName,
                model.LastName, DateTime.Now.ToString("dd-M-yyyy"), DateTime.Now.Millisecond, Extensions.Extensions.GetRndNumber());

            file.SaveAs(Path.Combine(Server.MapPath(model.PictureUrl)));

            return true;
        }

        //ovu funkciju ne koristim, ali cu je ostavit u slucaju ako neakd trebala
        private ChangeProfileViewModel FillChangeProfileViewModel(ApplicationUser temp)
        {
            ChangeProfileViewModel user = new ChangeProfileViewModel();
            user.Id = temp.Id;
            user.FirstName = temp.FirstName;
            user.LastName = temp.LastName;
            user.Username = temp.UserName;
            user.PhoneNumber = temp.PhoneNumber;
            user.IdentificationNumber = temp.IdentificationNumber;
            user.Email = temp.Email;
            user.Address = temp.Address;
            user.Township = temp.TownshipId;
            user.PictureUrl = temp.PictureUrl;

            return user;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
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

            var users = await UserManager.FindByNameAsync(model.Email);
            if (users != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(users.Id))
                {
                    ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View("Error");
                }
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        ApplicationUser user = context.Users.Where(u => u.Email == model.Email).FirstOrDefault();
                        if (!user.Status)
                        {
                            user.Status = true;
                            context.Users.Attach(user);
                            context.Entry(user).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
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

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.TownshipId = new SelectList(context.PlaceOfResidences, "Id", "Name");
            List<IdentityRole> roles = context.Roles.ToList();
            List<IdentityRole> rolesForView = new List<IdentityRole>();

            foreach (var item in roles)
            {
                if (item.Name == "Nakupac")
                {
                    rolesForView.Add(item);
                }
                if (item.Name == "Korisnik")
                {
                    rolesForView.Add(item);
                }
            }

            ViewBag.RoleID = new SelectList(rolesForView, "Id", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    RegistrationDate = DateTime.Now,
                    TownshipId = model.TownshipId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    IdentificationNumber = model.IdentificationNumber,
                    Status = false
                };

                var path = "/Images/UserImg/" + 
                    user.FirstName + user.LastName + "-" +user.IdentificationNumber;
                 DirectoryHelper dh = new DirectoryHelper(path);
                dh.Create();

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    string roleid = model.RoleID;
                    IdentityUserRole ur = new IdentityUserRole();

                    ur.RoleId = roleid;
                    ur.UserId = user.Id;
                    user.Roles.Add(ur);
                    context.SaveChanges();

                    string body = string.Format("Poštovani, {0} <BR/>Hvala Vam na registraciji, molimo da pratite link kako bi verifikovali Email: <a href=\"{1}\"title=\"User Email Confirm\">{1}</a>",
                    user.UserName, Url.Action("ConfirmEmail", "Account",
                        new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));

                    var emailClient = new EmailClient("Potvrda Email adrese", user.Email, body);
                    emailClient.Send();
                    
                   //await this.UserManager.AddToRoleAsync(user.Id, "Korisnik");

                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
                AddErrors(result);
            }
            return View(model);
        }
        
        [AllowAnonymous] //apa dodao
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.EmailConfirmed = true;
                    await UserManager.UpdateAsync(user);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //ovdje sam dodao rememberbrowser false
                    return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }
        }

        [AllowAnonymous] //apa dodao
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email; return View();
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
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                
                var body = string.Format("Poštovani, {0} resetirajte lozinku klikom na ovaj link: <a href=\"{1}\"> OVDJE </a>",
                user.UserName, callbackUrl);
                var emailClient = new EmailClient("Reset Lozinke", user.Email, body);
                emailClient.Send();

                //await this.UserManager.AddToRoleAsync(user.Id, "User");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

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
                // Don't reveal that the user does not exist
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
            // Request a redirect to the external login provider
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

            // Generate the token and send it
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

            // Sign in the user with this external login provider if the user already has a login
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
                    // If the user does not have an account, then prompt the user to create an account
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
                // Get the information about the user from the external login provider
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
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
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