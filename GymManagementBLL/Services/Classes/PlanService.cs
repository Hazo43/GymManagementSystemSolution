using AutoMapper;
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
        private readonly IMapper mapper;

        public PlanService( IUnitOfWork _unitOfWork , IMapper _mapper) 
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
      
        // GetAll()
        public IEnumerable<PlanViewModel> GetAllPlan()
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetAll();

            if (Plan == null || !Plan.Any())
                return [];

            var Planviewmodel = mapper.Map<IEnumerable<PlanViewModel>>(Plan);
            return Planviewmodel;
        }

        // GetPlanById => ( Get Plan Details )
        public PlanViewModel? GetPlanById(int id)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(id);

            if (Plan is null) return null;

            var planviewmodel = mapper.Map<PlanViewModel>(Plan);
            return planviewmodel;

        }
        // Update => Display 
        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            
            if (Plan is null || Plan.IsActive == false || HasActiveMemberShip(planId) )
               
                return null;

            var planviewmodel = mapper.Map<Plan , UpdatePlanViewModel>(Plan);
            return planviewmodel;
        }
        // Update Plan
        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null) return false;

            try
            {
                mapper.Map(updatedPlan, Plan);

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
