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
    public class trainerRepository : ItrainerRepository
    {
        private readonly GymDbContext dbContext;
        public trainerRepository(GymDbContext _dbContext)
        {
             dbContext = _dbContext;
        }
        public int Add(Trainer trainer)
        {
            dbContext.Trainers.Add(trainer);
            return dbContext.SaveChanges();
        }

        public int Delete(Trainer trainer)
        {
            dbContext.Trainers.Remove(trainer);
             return dbContext.SaveChanges();
        }

        public IEnumerable<Trainer> GetAll()
        {
            return dbContext.Trainers.ToList();
        }

        public Trainer? GetById(int Id)
           => dbContext.Trainers.Find(Id);

        public int Update(Trainer trainer)
        {
            dbContext.Trainers.Update(trainer);
            return dbContext.SaveChanges();
        }
    }
}
