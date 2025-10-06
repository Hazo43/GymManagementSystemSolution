using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        int Add (Plan plan);
        int Delete (Plan plan);
        int Update (Plan plan);
        Plan? GetById(int Id);
        IEnumerable<Plan> GetAll ();    
    }
}
