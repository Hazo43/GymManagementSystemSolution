using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{

    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService _sessionService)
        {
            sessionService = _sessionService;
        }

        #region Get All Sessions
        public ActionResult Index(int id)
        {
            var Session = sessionService.GetAllSession();
            return View(Session);
        }
        #endregion

        #region Get Session Details 

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Session = sessionService.GetSessionById(id);

            if (Session == null)
            {
                TempData["ErrorMessage"] = " Session Not Found ";
                return RedirectToAction(nameof(Index));
            }

            return View(Session);

        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            var Categories = sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

            var Trainers = sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
            return View();
        }


        [HttpPost]
        public ActionResult Create( CreateSessionViewModel createSession)
        {
            if(!ModelState.IsValid)
            {
                LoadForDropDownForCategories();
                LoadForDropDownForTrainers();
                return View(createSession);
            }

            bool Result = sessionService.CreateSession(createSession);
            if (Result == true)
            {
                TempData["SuccessMessage"] = " Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = " Session Failed To Create";
                LoadForDropDownForCategories();
                LoadForDropDownForTrainers();
                return View(createSession);
            }
 
        }

       

        #endregion

        #region Edit Session 

        public ActionResult Edit( int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = " Id Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Session = sessionService.GetSessionToUpdate(id);

            if(Session == null)
            {
                TempData["ErrorMessage"] = " Session Can Not Be Updated ";
                return RedirectToAction(nameof(Index));
            }

            LoadForDropDownForTrainers();
            return View(Session);

        }


        [HttpPost]
        public ActionResult Edit([FromRoute] int id , UpdateSessionViewModel updateSession)
        {
            if(ModelState.IsValid == false)
            {
             
                LoadForDropDownForTrainers();
                return View(updateSession);
            }
            
            var Result = sessionService.UpdateSession(id, updateSession);

            if(Result == true)
            {
                TempData["SuccessMessage"] = " Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = " Session Failed To Update";
            }

            return RedirectToAction(nameof(Index));
        }



        #endregion

        #region Delete Session 

        public ActionResult Delete(int id )
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Session = sessionService.GetSessionById(id);
          
            if(Session == null)
            {
                TempData["ErrorMessage"] = " Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = Session.Id;
            return View();
        }


        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = sessionService.RemoveSession(id);
            if(Result == true)
            {
                TempData["SuccessMessage"] = " Session Deleted Successfully";
            }
            else 
            {
                TempData["ErrorMessage"] = " Session FAiled To Deleted ";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion


        #region Helper 

        private void LoadForDropDownForCategories()
        {
            var Categories = sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        private void LoadForDropDownForTrainers()
        {
            var Categories = sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion
    }
}
