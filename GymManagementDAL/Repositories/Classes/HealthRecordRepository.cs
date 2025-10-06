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
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext dbContext;

        public HealthRecordRepository(GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public int Add(HealthRecord healthRecord)
        {
            dbContext.HealthRecords.Add(healthRecord);
            return dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var healthrecord = dbContext.HealthRecords.Find(id);
            if (healthrecord is null)
                return 0;
            else 
                dbContext.HealthRecords.Remove(healthrecord);
                return dbContext.SaveChanges();
                
        }

        public IEnumerable<HealthRecord> GetAll()

            => dbContext.HealthRecords.ToList();

        public HealthRecord? GetById(int id)

            => dbContext.HealthRecords.Find(id);

        public int Update(HealthRecord healthRecord)
        {
            dbContext.HealthRecords.Update(healthRecord);
            return dbContext.SaveChanges();
        }
    }
}
