using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService( IUnitOfWork _unitOfWork , IMapper _mapper) 
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                // Check If Trainer Exists
                if (!IsTrainerExists(createSession.TrainerId)) return false;

                // Check If Category Exists
                if (!IsCategoryExists(createSession.CategoryId)) return false;

                // Check StratDate Is before EndDate 
                if (!IsDateTimeValid(createSession.StartDate, createSession.EndDate)) return false;

                // Check Capacity 
                if (createSession.Capacity > 25 || createSession.Capacity < 0) return false;

                //var Session = new Session()
                //{
                //    CategoryId = createSession.CategoryId,
                //    TrainerId = createSession.TrainerId,
                //    Description = createSession.Description,
                //    Capacity = createSession.Capacity,
                //    StartDate = createSession.StartDate,
                //    EndDate = createSession.EndDate,
                //};

                var SessionEntity = mapper.Map<Session>(createSession);
                unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return unitOfWork.Savechanges() > 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($" Create Session Failed : {ex}");
                return false;
            }
           
        }

        public IEnumerable<SessionViewModel> GetAllSession()
        {
            var sessions = unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();
            if (sessions is null || !sessions.Any()) return [];

            //return sessions.Select(S => new SessionViewModel
            //{
            //    Id = S.Id,
            //    Description = S.Description,
            //    StartDate = S.StartDate,
            //    EndDate = S.EndDate,
            //    Capacity = S.Capacity,
            //    TrainerName = S.SessionTrainer.Name,
            //    CategoryName = S.SessionCategory.CategoryName,
            //    AvailableSlots = S.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(S.Id)
            //});

            /// بدل ما اعمل كل اللي فوق دا عملتهم في سطر واحد

            var mappingSassions = mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach ( var session in mappingSassions)
            {
                session.AvailableSlots = session.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return mappingSassions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var Session = unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if(Session is null) return null;

            var MappingSessions = mapper.Map<Session, SessionViewModel >(Session);

            MappingSessions.AvailableSlots = MappingSessions.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(MappingSessions.Id);
           
            return MappingSessions;
        }


        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = unitOfWork.SessionRepository.GetById(sessionId);
            // Helper 
            if (!ISessionAvailableForUpdating(Session)) return null;

            return mapper.Map<UpdateSessionViewModel>(Session);

            
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel updateSession)
        {
            try
                
            {
                var Sessions = unitOfWork.SessionRepository.GetById(sessionId);

                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!ISessionAvailableForUpdating(Sessions)) return false;
                if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;

                 //         Source           Destination  
                mapper.Map( updateSession , Sessions );
                
                Sessions.UpdatedAt = DateTime.Now;
                unitOfWork.SessionRepository.Update(Sessions);
                return unitOfWork.Savechanges() > 0;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Created Session Failed{ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Sessions = unitOfWork.SessionRepository.GetById(sessionId) ;
                
                // اي سيشن مكتمله همسحها يعني يكون مخلص السيشن غير كدا متمسحش

                if(Sessions is null ) return false;

                // لو السيشن شغاله متمسحهاش
                // If Session Started - No Deleted Allowed 
                if(Sessions.StartDate <= DateTime.Now &&  Sessions.EndDate > DateTime.Now) return false;
            
                // If Session Upcoming - No Deleted Allowed 
                if (Sessions.StartDate > DateTime.Now) return false;

                // If Session 
                var HasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(sessionId) > 0 ;
                if(!HasActiveBooking) return false;

                unitOfWork.SessionRepository.Delete(Sessions);
                return unitOfWork.Savechanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Remove Session Failed : {ex}");
                return false;
            }


        }

        #region Helper 

        private bool ISessionAvailableForUpdating(Session session)
        {
            // if Session Completed - No Updated Allowed
            if (session.EndDate < DateTime.Now) return false;
         
            // if Session Started - No Updated Allowed
            if (session.StartDate <= DateTime.Now)  return false; 
          
            // if Session Has Active Booking - No Updated Allowed

            var HasActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if(HasActiveBooking) return true;

            return true;
        }



        private bool IsTrainerExists(int TrainerId)
        {
            // False هيرجع null لو ب True هيرجع null لو مش ب
            
            return unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }
        private bool IsCategoryExists(int CategoryId)
        {
            // False هيرجع null لو ب True هيرجع null لو مش ب
           
            return unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }
        private bool IsDateTimeValid( DateTime StartDate , DateTime EndDate)
        {
            // StartDate قبل ال EndDate عشان True في الحاله دي هيرجع 

            return StartDate < EndDate; 
             
        }


        #endregion
    }
}
