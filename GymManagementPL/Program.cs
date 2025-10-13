using GymManagementBLL;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
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
            
            
            var app = builder.Build();

            #region Migrate Database - and - Data Seeding

            using var Scope = app.Services.CreateScope();
             var dbcontext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            
            /// seeding ﬁÌœ «·«‰ Ÿ«— Ì⁄‰Ì ·”Â „ ⁄„· ‘ ÂÌ—ÊÕ Ì‰›–Â« ﬁ»· „« Ì⁄„· migrations ·Ê ›ÌÂ check » ⁄„· 
          
            var PendingMigraions = dbcontext.Database.GetPendingMigrations();
            if(PendingMigraions?.Any() ?? false)
            {
                dbcontext.Database.Migrate();
            }

            GymDbContextSeeding.SeedData(dbcontext);

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

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
