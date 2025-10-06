using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = null!; // فصيله الدم
        public string? Note { get; set; }

        // LastUpdate == UpdatedAt => (inheritance) From BaseEntity

        #region RelationShip

        // Member هيتحول في نفس الجدول في ال


        #endregion

    }
}
