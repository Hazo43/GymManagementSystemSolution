﻿using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IHealthRecordRepository
    {
        int Add (HealthRecord healthRecord);
        int Update (HealthRecord healthRecord); 
        int Delete (int id);
        HealthRecord? GetById (int id);
        IEnumerable<HealthRecord> GetAll ();
    }
}
