using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository
    {
        IEnumerable<MemberShip> GetAll();
        MemberShip? GetById(int Id);
        int Add (MemberShip membership);
        int Update (MemberShip membership);
        int Delete (int Id);
    }
}
