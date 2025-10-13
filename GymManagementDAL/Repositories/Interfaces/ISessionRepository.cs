using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        // Trainer and Category  لل include احنا عملناها عشان نعمل 

        IEnumerable<Session> GetAllSessionWithTrainerAndCategory();

        int GetCountOfBookedSlots(int sessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
    }
}
