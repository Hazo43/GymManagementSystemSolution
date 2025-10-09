using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUserv 
    {
        // HireDate == CteatedAt => (inheritance) From BaseEntity
        public Specialties Specialties { get; set; }

        #region RelationShips 

        #region Session - Trainer 

        public ICollection<Session> TrainerSessions { get; set; } = null!;

        #endregion

        #endregion

    }
}
