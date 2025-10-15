using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService analyticsService;

        /// <summary>
        /// براحتي data و هحط ال return ل اي واحد من اللي تحت دول هعمل  return ف انا لو عاوز اعمل  ActionResult كل اللي عملتهم تحت دول وارثين من دا
        /// </summary>
        /// <returns></returns>

        public HomeController(IAnalyticsService _analyticsService)
        {
            analyticsService = _analyticsService;
        }
        public ActionResult Index()
        {
            var Data = analyticsService.GetAnalyticsData();
            return View(Data);
        }
        
        //public ViewResult Index()
        //{
        //    /// => ده هو هو اللي تحت بس اللي تحت اسهل 
        //    //var viewResult = new ViewResult();
        //    //return viewResult;

        //    return View();
        //}
        //public JsonResult Trainers()
        //{
        //    var Trainers = new List<Trainer>()
        //    {
        //        new Trainer(){ Name = "Ali" , Phone = "01207239250"},
        //        new Trainer(){ Name = "Amr" , Phone = "01002895186"}
        //    };
        //    return Json(Trainers);
        //}

        //public RedirectResult Redirect()
        //{
        //    return Redirect("https://chatgpt.com/c/68ec34d7-9df4-8325-823f-fbd1b174e06f");
        //}

        //public ContentResult Content()
        //{
        //    // html هيبدا ان هو يعرضها ك
        //    // ContentType => "text/html"
        //    return Content("<h1>Hello From Gym Management System</h1>" , "text/html");
        //}

        //public FileResult DownloadFile()
        //{
        //    var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
        //    var FileBytes = System.IO.File.ReadAllBytes(FilePath);
        //    //             Data    ContectType  DownloadName        
        //    return File(FileBytes, "text/css", "DownloadableSite.css");
        //} 
        
        ///// <summary>
        /////  مش بيرجع اي داتا بيرجع صفحه فاضيه
        ///// </summary>
        ///// <returns></returns>
        //public EmptyResult EmptyAction()
        //{
        //    return new EmptyResult();
        //}

    }
}
