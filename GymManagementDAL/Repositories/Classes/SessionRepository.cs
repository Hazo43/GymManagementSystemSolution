using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session> ,ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository( GymDbContext _dbContext) : base(_dbContext) 
        {
            dbContext = _dbContext;
        }

        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
            //  // Trainer and Category  لل include احنا عملناها عشان نعمل
           
            return dbContext.Sessions.Include( x => x.SessionTrainer)
                                     .Include( x => x.SessionCategory)
                                     .ToList(); 
        }


        public int GetCountOfBookedSlots(int sessionId)
        {
            // 
            return dbContext.MemberSessions.Count( x => x.SessionId == sessionId );
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
          return  dbContext.Sessions.Include( x => x.SessionTrainer)
                                    .Include( x => x.SessionMembers)
                                    .FirstOrDefault( x => x.Id == sessionId );
        }
    }
}
