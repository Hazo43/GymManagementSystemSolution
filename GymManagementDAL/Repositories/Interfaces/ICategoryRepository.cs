using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
         IEnumerable<Category> GetAll();
        Category? GetById(int Id);

        // Create 
        int Add (Category category);
        int Update (Category category);
        int Delete (Category category);
    }
}
