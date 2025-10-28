using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewMpdels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{

    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService planService;

        public PlanController( IPlanService _planService)
        {
            planService = _planService;
        }

        #region Get All Plans

        public IActionResult Index()
        {
            var Plan = planService.GetAllPlan();
            return View(Plan);
        }

        #endregion

        #region Get Plan Details 
        
        public IActionResult Details( int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = " Id Of Trainer Can Not Be 0 Or Negative Namber";
                return RedirectToAction(nameof(Index));
            }

            var Plan = planService.GetPlanById(id);

            if(Plan == null)
            {
                TempData["ErrorMessage"] = " Plan Not Found ";
                return RedirectToAction(nameof(Index));
            } 
            return View(Plan);
            
        }

        #endregion

        #region Plan Edit

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
           
            var Plans = planService.GetPlanToUpdate(id);

            if(Plans == null)
            {
                TempData["ErrorMessage"] = " Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Plans);
        }


        [HttpPost]
        public ActionResult Edit([FromRoute] int id , UpdatePlanViewModel updatePlanView)
        {
            if(ModelState.IsValid == false)
            {
                ModelState.AddModelError("WrongData" , "Check Data Validation");
                return View(updatePlanView);
            }

            var Result = planService.UpdatePlan(id, updatePlanView);

            if (Result == true)
            {
                TempData["SuccessMessage"] = " Plan Update Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Faild To Update";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Plan 

        [HttpPost]
        public ActionResult Activate(int id)
        {

            var Result = planService.ToggleStatus(id);

            if (Result == true)
                TempData["SuccessMessage"] = "Plan Status Changes";
            else
                TempData["ErrorMessage"] = " Failed To Change Plan Status ";

            return RedirectToAction(nameof(Index));

        }

        #endregion
    }
}
