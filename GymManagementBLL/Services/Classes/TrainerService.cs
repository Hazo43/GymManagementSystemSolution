using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService( IUnitOfWork _unitOfWork , IMapper _mapper) 
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Email = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == createTrainer.Email).Any();
                if (Email == true) return false;
                var Phone = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == createTrainer.Phone).Any();
                if (Phone == true) return false;

                var Trainer = mapper.Map<CreateTrainerViewModel, Trainer>(createTrainer);

                unitOfWork.GetRepository<Trainer>().Add(Trainer);
                return unitOfWork.Savechanges() > 0;
            }
            catch
            {
                return false;
            }


           
        }

        public IEnumerable<TrainerViewModel> GetAllTrainer()
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainer is null || !Trainer.Any()) return [];

            var trainerviewmodel = mapper.Map<IEnumerable<TrainerViewModel>>(Trainer);
            return trainerviewmodel;
            
        }

        public TrainerViewModel? GetTrainerDetails(int trainerid)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if(Trainer is null ) return null;

            var TrainerViewModel = mapper.Map<TrainerViewModel>(Trainer);
            return TrainerViewModel;
        }

        public UpdateTrainerViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if(Trainer is null ) return null;

            var viewModel = mapper.Map< Trainer ,UpdateTrainerViewModel>(Trainer);
            return viewModel;
        }

        public bool UpdateTrainer(int trainerid, UpdateTrainerViewModel updateTrainer)
        {
            var Email = unitOfWork.GetRepository<Trainer>().GetAll( x => x.Email == updateTrainer.Email );
            var Phone = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == updateTrainer.Phone);
            if( Email is null  || Phone is null) return false;

            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if( Trainer is null ) return false;

            mapper.Map(updateTrainer , Trainer);

            unitOfWork.GetRepository<Trainer>().Update(Trainer);
            return unitOfWork.Savechanges() > 0;

        }

        public bool DeleteTrainer(int trainerid)
        {
            var TrainerToRemove = unitOfWork.GetRepository<Trainer>().GetById(trainerid);

            if (TrainerToRemove == null || HasActiveSessions(trainerid)) return false;

            unitOfWork.GetRepository<Trainer>().Update(TrainerToRemove);
            return unitOfWork.Savechanges() > 0;
        }

            #region Helper 

         /// <summary>
         /// بتاعها لسه مجاش متمسحهاش startDate اي سيشن ال
         /// </summary>
         /// <param name="trainerid"></param>
         /// <returns></returns>
            
            private bool HasActiveSessions(int trainerid)
            {
              var HasActiveSessions = unitOfWork.GetRepository<Session>()
             .GetAll(s => s.TrainerId == trainerid && s.StartDate > DateTime.Now).Any(); 
              return HasActiveSessions;
            }
           

            #endregion
        
    }
}
