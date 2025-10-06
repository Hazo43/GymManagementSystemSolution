using GymManagementDAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    // Trainers و Members هيكون جواه الحجات الشائعه اللي مبين ال
    public abstract class GymUserv : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; } 
        public Gender Gender { get; set; }

        public Address Address { get; set; } = null!;

    }

    // GymUserv بتاع ال  class عشان كدا عملناه جوا ال GymUserv مملوك بس ل Address ال
  
    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;

    }

}
