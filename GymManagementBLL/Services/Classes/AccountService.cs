using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountService(UserManager<ApplicationUser> _userManager )
        {
            userManager = _userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            // Check Email 
           var User = userManager.FindByEmailAsync( loginViewModel.Email ).Result;
            if ( User == null ) 
                return null;
            // Check Password
            var IsPasswordValid = userManager.CheckPasswordAsync( User , loginViewModel.Password ).Result;
            if( IsPasswordValid == true ) 
                return User;
            else
                return null;
        }
    }
}
