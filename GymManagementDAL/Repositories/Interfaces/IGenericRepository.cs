﻿using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        TEntity? GetById (int Id);
        IEnumerable<TEntity> GetAll(Func<TEntity , bool>? condition = null);
        int Add (TEntity entity);
        int Update (TEntity entity);
        int Delete (TEntity entity);
    }
}
