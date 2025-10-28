using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedDataUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                // false ولا لا لو عندو هرجع Roles  و Users جوا ال data هل عندو  Check بعمل 
                var HasUsers = userManager.Users.Any();
                var HAsRoles = roleManager.Roles.Any();
                // false رجع  data لو عندو 
                if (HasUsers && HAsRoles == true) return false;
                //  رجع اللي جواها بق  data لو معندوش 
                if (!HAsRoles)
                {
                    // Roles هضيف عندي اتنين 
                    var Roles = new List<IdentityRole>()
                    {
                        new() { Name = "SuperAdmin"},
                        new() { Name = "Admin"}
                    };

                    // Roles هضيفهم جوا ال
                    foreach (var role in Roles)
                    {
                        // ولا لا RoleExistsAsync هو ال check  هعمل 
                        // true تبقي  false ال  Not !  يبقي  false لو رجعت 
                        // create هبدا اعمل بقا 
                        if (!roleManager.RoleExistsAsync(role.Name).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                        // userManager هعمل نفي الكلام مع ال
                    }


                }

                // Two Admin هيبقي عندنا
                if (!HasUsers)
                {
                    // اول واحد
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Abdel-Rahmen",
                        LastName = "Gaber",
                        UserName = "Abdel-RahmenGaber",
                        Email = "Abdel-RahmenGaber@gmail.com",
                        PhoneNumber = "01207239250",
                    };
                   
                    // Hashing عشان ال  Password وهنا ببعت معاها ال Create هروح اعملو 
                    userManager.CreateAsync(MainAdmin , "P@ssw0rd").Wait();

                    // يعني Role هضيف ليه  SuperAdmin هو ال Abdel-Rahmen هخلي 
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();



                    // تاني واحد
                    var Admin = new ApplicationUser()
                    {
                        FirstName = "’Mahmoud",
                        LastName = "Gaber",
                        UserName = "MahmoudGaber",
                        Email = "MahmoudGaber@gmail.com",
                        PhoneNumber = "01210882408",
                    };

                    // Hashing عشان ال  Password وهنا ببعت معاها ال Create هروح اعملو 
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();

                    // يعني Role هضيف ليه  Admin هو ال Mahmoud هخلي 
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();

                }


                return true;
            }
             catch (Exception ex)  
            {
                Console.WriteLine($"Seed Failed {ex}");
                return false;
            }
        }
    }
}
