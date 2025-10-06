using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        // GetAll
        IEnumerable<Member> GetAll();

        // Get by Id 
        Member? GetById(int Id);
        
        // Add 
        int Add (Member member);

        // Update
        int Update (Member member);

        // Delete 
        int Delete (int Id);
    }
}
