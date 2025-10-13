using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewMpdels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlanService( IUnitOfWork _unitOfWork) 
        {
            unitOfWork = _unitOfWork;
        }
      
        // GetAll()
        public IEnumerable<PlanViewModel> GetAllPlan()
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetAll();

            if (Plan == null || !Plan.Any())
                return [];

            var Planviewmodel = Plan.Select(x => new PlanViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DurationDays = x.DurationDays,
                IsActive = x.IsActive,
                Price = x.Price,
            });
            return Planviewmodel;
        }

        // GetPlanById => ( Get Plan Details )
        public PlanViewModel? GetPlanById(int id)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(id);

            if (Plan is null) return null;

            var planviewmodel = new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price,
            };
            return planviewmodel;

        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            
            if (Plan is null || Plan.IsActive == false || HasActiveMemberShip(planId) )
               
                return null;

            var planviewmodel = new UpdatePlanViewModel()
            {
                Price = Plan.Price,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                PlanName = Plan.Name,
            };
            return planviewmodel;
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null) return false;

            try
            {
                Plan.Description = updatedPlan.Description;
                Plan.Price = updatedPlan.Price;
                Plan.DurationDays = updatedPlan.DurationDays;
                Plan.UpdatedAt = DateTime.Now;

                unitOfWork.GetRepository<Plan>().Update(Plan);
                return unitOfWork.Savechanges() > 0;
            }
            catch 
            {
                return false;
            }
        }

        public bool ToggleStatus(int planId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null || HasActiveMemberShip(planId)) return false;

            if (Plan.IsActive == true)
                Plan.IsActive = false; 
            
            else if(Plan.IsActive == false) 
                Plan.IsActive = true;

            // اعمل كدا  Update لازم مع كل
            
            Plan.UpdatedAt = DateTime.Now;

            try 
            {
                unitOfWork.GetRepository<Plan>().Update(Plan);
                return unitOfWork.Savechanges() > 0;
            }
            catch 
            {
                return false;
            }
        }


        #region Helper 
      
        /// <summary>
        /// MemberShip في ال Active لو جيالي حاجه بالمواصفات دي يبقي كدا هو عندو
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>

        private bool HasActiveMemberShip (int planId)
        {
            var ActiveMemberShip = unitOfWork.GetRepository<MemberShip>()
                                  .GetAll(x => x.PlanId == planId
                                          && x.Status == "Active");
            return ActiveMemberShip.Any();
        }

        #endregion
    }
}
