using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAccountService _accountService, SignInManager<ApplicationUser> _signInManager)
        {
            accountService = _accountService;
            signInManager = _signInManager;
        }

        #region Login 

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var User = accountService.ValidateUser(model);
            if (User is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }

            // بتاعتنا Sign In دي 
            var Result = signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false).Result;

            // صح بس هو مش مسموح ليه يخش عندي Email و  Password دي معنها ان هو عندي و ال
            if (Result.IsNotAllowed)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account Is Not Allowed");
            }
            // هو الاكونت بتاعو مغلق ولا لا Check دي بتعمل
            if (Result.IsLockedOut)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account Is Locked Out");
            }
            // في حاله ان هو صح
            if (Result.Succeeded)
            {
                //                      Action , Controller
                return RedirectToAction("Index", "Home");
            }

            // View(model) في حاله ان هو منفذش اي حاجه من اللي فوق دول روح رجعلو ال
            return View(model);

        }

        #endregion

        #region Logout

        public ActionResult Logout()
        {
            signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region Access Denied 


        public ActionResult AccessDenied()
        {
            return View();
        }

        #endregion


    }
}
