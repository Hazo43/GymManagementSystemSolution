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
    public class MemberRepository : IMemberRepository
    {
        // Database مع ال Connection دا ال
        // DbContext Object Is Injected , Not Created Manually  

        private readonly GymDbContext dbContext;

        // Ask CLR To Inject Object From GymDbContext
        // 
        public MemberRepository( GymDbContext _dbContext)
        {
             dbContext = _dbContext;
        }

        public int Add(Member member)
        {
            dbContext.Members.Add(member);
            return dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var member = dbContext.Members.Find(Id);
            if(member is null)
                return 0;
            else 
               dbContext.Members.Remove(member);
                 return dbContext.SaveChanges();
            
        }

        public IEnumerable<Member> GetAll()
        {
            return dbContext.Members.ToList();
        }

        public Member? GetById(int Id)
        {
            return dbContext.Members.Find(Id);
        }

        public int Update(Member member)
        {
            dbContext.Members.Update(member);
            return dbContext.SaveChanges();
        }
    }
}
