﻿using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepoditory
    {
        int Add (Session session);
        int Update (Session session);
        int Delete (Session session);
        IEnumerable<Session> GetAll ();
        Session? GetById (int Id);

    }
}
