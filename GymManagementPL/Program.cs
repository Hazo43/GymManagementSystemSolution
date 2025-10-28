using GymManagementBLL;
using GymManagementBLL.Services.AttachmemtService;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>( options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped(typeof(IPlanRepository), typeof(PlanRepository));
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService , TrainerService>();
            builder.Services.AddScoped<IPlanService , PlanService>();
            builder.Services.AddScoped<ISessionService , SessionService>();
            builder.Services.AddScoped<IAttachmentService  , AttachmentService>();
            builder.Services.AddScoped<IAccountService , AccountService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>( config =>
            {
                // by default œÊ· „„ﬂ‰ «„”ÕÂ„ Â„« „ÊÃÊœÌ‰ «’·«
                //config.Password.RequiredLength = 6;
                //config.Password.RequireLowercase = true;
                //config.Password.RequireUppercase = true;

                // Email Unique
                config.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<GymDbContext>();

            // by default „„ﬂ‰ „⁄„·Â«‘ «’·« ·«‰ ÂÌÂ „ÊÃÊœÂ
            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Login „‘ ⁄«„· user œÌ ÂÌ—ÊÕ ⁄·ÌÂ ·Ê «·
                options.LoginPath = "/Account/Login";
                // „‘ „”„ÊÕ ·ÌÂ » «·Õ«ÃÂ «··Ì ÂÊ —«ÌÕ ⁄·ÌÂ« user  œÌ ÂÌ—ÊÕ ⁄·ÌÂ« ·Ê «·
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            #region Migrate Database - and - Data Seeding

            using var Scope = app.Services.CreateScope();
             var dbcontext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            // 
            var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            /// seeding ﬁÌœ «·«‰ Ÿ«— Ì⁄‰Ì ·”Â „ ⁄„· ‘ ÂÌ—ÊÕ Ì‰›–Â« ﬁ»· „« Ì⁄„· migrations ·Ê ›ÌÂ check » ⁄„· 
          
            var PendingMigraions = dbcontext.Database.GetPendingMigrations();
            if(PendingMigraions?.Any() ?? false)
            {
                dbcontext.Database.Migrate();
            }

            GymDbContextSeeding.SeedData(dbcontext);
            // 
            IdentityDbContextSeeding.SeedDataUser(roleManager ,  userManager);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
