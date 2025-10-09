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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext dbContext;
        public GenericRepository( GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(TEntity entity)
               => dbContext.Set<TEntity>().Add(entity);
        public void Delete(TEntity entity)
                => dbContext.Set<TEntity>().Remove(entity);

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition == null)
                return dbContext.Set<TEntity>().AsNoTracking().ToList();
            else 
                return  dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
        }

        public TEntity? GetById(int Id)
              => dbContext.Set<TEntity>().Find(Id);

        public void Update(TEntity entity)
                     => dbContext.Set<TEntity>().Remove(entity);
    }
}
