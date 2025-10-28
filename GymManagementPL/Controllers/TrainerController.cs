using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{

    [Authorize( Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService _trainerService)
        {
            trainerService = _trainerService;
        }

        #region Get All Trainers

        public ActionResult Index()
        {
            var Trainer = trainerService.GetAllTrainer();
            return View(Trainer);
        }

        #endregion

        #region Get Trainer Details 

        public ActionResult TarinerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Trainer Can Not Be 0 Or Negative Namber";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = trainerService.GetTrainerDetails(id);

            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer NoT Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Trainer);

        }

        #endregion

        #region Create 

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields");
                return View(createTrainer);
            }

            bool Result = trainerService.CreateTrainer(createTrainer);

            if (Result == true)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Create";
                
            }

            return RedirectToAction(nameof(Create));
        }
        #endregion

        #region Edit Tarainer 

        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Trainer = trainerService.GetTrainerToUpdate(id);

            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Trainer);
        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id , UpdateTrainerViewModel updateTrainer)
        {
            if(ModelState.IsValid == false)
            {
                return View(updateTrainer);
            }

            var Result = trainerService.UpdateTrainer(id , updateTrainer);

            if(Result == true)
            {
                TempData["SuccessMessage"] = "Trainer Update Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Update";
            }

            return RedirectToAction(nameof(Index));
        }



        #endregion

        #region Delete Trainer 

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var Trainer = trainerService.GetTrainerDetails(id);

            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
          
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = Trainer.Name;
           
            return View();
        }



        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = trainerService.DeleteTrainer(id);

            if (Result == true)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Trainer Faild To Delete ";

            return RedirectToAction(nameof(Index));
        }


        #endregion

    }
}
