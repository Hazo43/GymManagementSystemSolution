using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace GymManagementPL.Controllers
{

    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController( IMemberService _memberService)
        {
            memberService = _memberService;
        }

        #region Get All Members 

        // Member/Index
        public ActionResult Index()
        {
          var Member = memberService.GetAllMembers();
            return View(Member);
        }
        #endregion

        #region Get Member Details 

        // Member/MemberDetails/id
        public ActionResult MemberDetails(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));

            }
                

            var Member = memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member NoT Found";
                return RedirectToAction(nameof(Index));
            }
               

            return View(Member);

        }

        #endregion

        #region Get HealthRecord Details

        // Member/HealthRecordDetails/id 
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var HealthRecordDetails = memberService.GetMemberHealthRecordDetails(id);

            if(HealthRecordDetails is null)
            {
                TempData["ErrorMessage"] = "Health Record Nor Found";
                return RedirectToAction(nameof(Index));
            }
               

            return View(HealthRecordDetails);
        }


        #endregion

        #region Create Member 

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember (CreateMemberViewModel CreatedMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields");
                return View(nameof(Create));
            }

            bool Result = memberService.CreateMember(CreatedMember);

            if (Result == true)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Faild To Create , Check Phone And Email" ;
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Edit Member 

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            
            var member = memberService.GetMemberToUpdate(id);
            
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdid([FromRoute] int id , MemberToUpdateViewModel memberToUpdate)
        {
            if(ModelState.IsValid == false)
            {
                return View(memberToUpdate);
            }

            var Result = memberService.UpdateMember(id, memberToUpdate);

            if (Result == true)
            {
                TempData["SuccessMessage"] = "Member Update Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Faild To Update";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member 

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = memberService.GetMemberDetails(id);

            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result = memberService.RemoveMember(id);
            if (Result == true)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Member Faild To Delete " ;

            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
