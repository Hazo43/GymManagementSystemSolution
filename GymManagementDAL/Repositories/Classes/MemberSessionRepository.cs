using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class MemberSessionRepository : IMemberSessionRepository
    {
        private readonly GymDbContext dbContext;
        public MemberSessionRepository( GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int Add(MemberSession memberSession)
        {
           dbContext.MemberSessions.Add(memberSession);
            return dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var membersession = dbContext.MemberSessions.Find(Id);
            if (membersession is null)
                return 0;
            else 
                dbContext.MemberSessions.Remove(membersession);
                return dbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetAll()
             
            => dbContext.MemberSessions.ToList();

        public MemberSession? GetById(int Id)

            => dbContext.MemberSessions.Find(Id);

        public int Update(MemberSession memberSession)
        {
            dbContext.MemberSessions.Update(memberSession);
            return dbContext.SaveChanges();
        }
    }
}
