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

        public TrainerService( IUnitOfWork _unitOfWork) 
        {
            unitOfWork = _unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Email = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == createTrainer.Email).Any();
                if (Email == true) return false;
                var Phone = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == createTrainer.Phone).Any();
                if (Phone == true) return false;

                var Trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Gender = createTrainer.Gender,
                    DateOfBirth = createTrainer.DataOfBirth,
                    Specialties = createTrainer.Specialties,

                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        City = createTrainer.City,
                        Street = createTrainer.Street,
                    }
                };
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

            var trainerviewmodel = Trainer.Select(x => new TrainerViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Specialties = x.Specialties,
            });
            return trainerviewmodel;
            
        }

        public TrainerViewModel? GetTrainerDetails(int trainerid)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if(Trainer is null ) return null;

            var TrainerViewModel = new TrainerViewModel()
            {
                
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                DateOfBirth = Trainer.DateOfBirth,
                Address = Trainer.Address.ToString(),
                Specialties = Trainer.Specialties
            };
            return TrainerViewModel;
        }

        public TrainerToUpdateViewModel? GetMemberToUpdate(int trainerId)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if(Trainer is null ) return null;

            var viewModel = new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name,
                Phone = Trainer.Phone,
                Email = Trainer.Email,
                Specialties = Trainer.Specialties,
                BuildingNumber = Trainer.Address.BuildingNumber,
                Street = Trainer.Address.Street,
                City = Trainer.Address.City,
            };
            return viewModel;
        }

        public bool UpdateTrainer(int trainerid, TrainerToUpdateViewModel updateTrainer)
        {
            var Email = unitOfWork.GetRepository<Trainer>().GetAll( x => x.Email == updateTrainer.Email );
            var Phone = unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == updateTrainer.Phone);
            if( Email is null  || Phone is null) return false;

            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerid);
            if( Trainer is null ) return false;

            Trainer.Name = updateTrainer.Name;
            Trainer.Email = updateTrainer.Email;
            Trainer.Phone = updateTrainer.Phone;
            Trainer.Address.BuildingNumber = updateTrainer.BuildingNumber;
            Trainer.Address.Street = updateTrainer.Street;
            Trainer.Address.City = updateTrainer.City;
            Trainer.Specialties = updateTrainer.Specialties;

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
