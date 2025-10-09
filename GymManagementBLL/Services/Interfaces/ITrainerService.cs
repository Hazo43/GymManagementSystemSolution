using GymManagementBLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainer();

        bool CreateTrainer(CreateTrainerViewModel createTrainer);

        TrainerViewModel? GetTrainerDetails(int trainerid);
        
        TrainerToUpdateViewModel? GetMemberToUpdate(int trainerId);
        bool UpdateTrainer (int  trainerid, TrainerToUpdateViewModel updateTrainer);

        bool DeleteTrainer(int trainerid);
    }
}
