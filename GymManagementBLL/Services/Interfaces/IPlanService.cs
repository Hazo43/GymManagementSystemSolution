using GymManagementBLL.ViewModels.PlanViewMpdels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlan();

        PlanViewModel? GetPlanById (int id);

        UpdatePlanViewModel? GetPlanToUpdate(int planId);
       
        bool UpdatePlan ( int planId, UpdatePlanViewModel updatedPlan );

        bool ToggleStatus(int  planId);

    }
}
