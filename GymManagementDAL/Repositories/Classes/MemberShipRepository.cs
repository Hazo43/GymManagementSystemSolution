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
    public class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDbContext dbContext;
        public MemberShipRepository( GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int Add(MemberShip membership)
        {
            dbContext.MemberShips.Add(membership);
            return dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var membership = dbContext.MemberShips.Find(Id);
            if (membership is null)
                return 0;
            else 
                dbContext.MemberShips.Remove(membership);
                return dbContext.SaveChanges();
        }

        public IEnumerable<MemberShip> GetAll()

            => dbContext.MemberShips.ToList();

        public MemberShip? GetById(int Id)

            => dbContext.MemberShips.Find(Id);

        public int Update(MemberShip membership)
        {
            dbContext.MemberShips.Update(membership);
            return dbContext.SaveChanges();
        }
    }
}
