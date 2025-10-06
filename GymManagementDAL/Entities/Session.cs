﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Session: BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region RelationShip

        #region Session - Category 

        public Category SessionCategory { get; set; } = null!;
        public int CategoryId { get; set; }


        #endregion

        #region Session - Trainer 

        public Trainer SessionTrainer { get; set; } = null!;
        public int TrainerId { get; set; }

        #endregion

        #region Seesion - MemberSession 

        public ICollection<MemberSession> SessionMembers { get; set; } = null!;


        #endregion

        #endregion

    }
}
