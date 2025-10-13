using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext dbContext;

        private readonly Dictionary<Type, Object> reposirories = [];
        public UnitOfWork( GymDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public ISessionRepository SessionRepository
                              => new SessionRepository(dbContext);

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);

            if(reposirories.TryGetValue(EntityType, out var Repo)) 
                return (IGenericRepository<TEntity>)Repo;

            var repository = new GenericRepository<TEntity>(dbContext);

            reposirories.Add(EntityType , repository);

            return repository;
        }

        public int Savechanges()
               => dbContext.SaveChanges();
    }
}
