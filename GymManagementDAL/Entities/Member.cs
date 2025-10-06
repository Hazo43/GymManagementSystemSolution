using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUserv
    {
        // JoinDate == CreatedAt => (inheritance) from BaseEntity
        public string? Photo { get; set; } // Photo بس لل URL او ال Name هنخزن ال 

        #region RelationShip

        #region 1 - Member ->  (Mandatory) , 1 - HealthRecord -> (Mandatory)
        public HealthRecord HealthRecord { get; set; } = null!;

        // Fk كا Member بتاع ال  PK هنا هيكون ال

        #endregion

        #region Member - MemberShip 
        public ICollection<MemberShip> MemberShips { get; set; } = null!;

        #endregion

        #region Member - MemberSession

        public ICollection<MemberSession> memberSessions { get; set; } = null!;

        #endregion

        #endregion

    }
}
