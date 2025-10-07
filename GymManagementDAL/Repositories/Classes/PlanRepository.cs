﻿using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository 
    {
        private readonly GymDbContext dbContext;

        public PlanRepository( GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IEnumerable<Plan> GetAll()
          
            => dbContext.Plans.ToList();

        public Plan? GetById(int Id)
        {
            return dbContext.Plans.Find(Id);
        }

        public int Update(Plan plan)
        {
            dbContext.Plans.Update(plan);
            return dbContext.SaveChanges();
        }
    }
}
