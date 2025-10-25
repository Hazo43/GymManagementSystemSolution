using AutoMapper;
using GymManagementBLL.Services.AttachmemtService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAttachmentService attachmentService;


        // Ask SLR For Creating Object From Services   => Services يعني لازم اروح البروجرم واعمل حجات في ال
        // CLR Will Inject Address of Object In Constractor 
        public MemberService( IUnitOfWork _unitOfWork , IMapper _mapper , IAttachmentService _attachmentService)
        {
           unitOfWork = _unitOfWork;
            mapper = _mapper;
            attachmentService = _attachmentService;
        }

        // Add - Create 
        public bool CreateMember(CreateMemberViewModel CreateMember)
        {
            try
            {
                // Check If Email Is Exists 
                var emailExists = unitOfWork.GetRepository<Member>().GetAll(x => x.Email == CreateMember.Email).Any();
                if (emailExists == true) return false;

                // Check If Phone Is Exists 
                var phoneExists = unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == CreateMember.Phone).Any();
                if (phoneExists == true) return false;

                //
                var PhotoName = attachmentService.Upload("members", CreateMember.PhotoFile);
                if(string.IsNullOrEmpty(PhotoName)) return false;
              
                var member = mapper.Map<CreateMemberViewModel , Member>(CreateMember);
                
                member.Photo = PhotoName;


                unitOfWork.GetRepository<Member>().Add(member) ;

                var IsCreated = unitOfWork.Savechanges() > 0 ;
                if(!IsCreated)
                {
                    attachmentService.Delete(PhotoName, "members");
                    return false;
                }

                return IsCreated;
            
            }
            catch 
            {
              return false;
            }

     
        }

        // Get All  => Display Data For User 
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Member = unitOfWork.GetRepository<Member>().GetAll();   // => IGenericRepository<TEntity> 
           
            if (Member is null || !Member.Any()) 
                  return [];


            var membersmodels = mapper.Map<IEnumerable<MemberViewModel>>(Member);
            return membersmodels;
        }

        // Get Member Details
        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(MemberId);

            if (member is null) return null;

            var viewModel = mapper.Map<MemberViewModel>(member);

            // Active Membership 
            var Activemembership = unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId 
                                                       && x.Status == "Active").FirstOrDefault();

            if (Activemembership is not null)
            {
                viewModel.MembershipStartDate = Activemembership.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = Activemembership.EndDate.ToShortDateString();

                var plan = unitOfWork.GetRepository<Plan>().GetById(Activemembership.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        // Get Member Health Record Details 
        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var memberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);

            if (memberHealthRecord is null) return null;

            var viewModel = mapper.Map<HealthRecordViewModel>(memberHealthRecord);

            return viewModel;
        }

        // Update 
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;

            return mapper.Map<MemberToUpdateViewModel>(member);
        }
        public bool UpdateMember(int Id, MemberToUpdateViewModel UpdateMember)
        {
            var Email = unitOfWork.GetRepository<Member>()
                                  .GetAll( x => x.Email == UpdateMember.Email && x.Id != Id);
            var Phone = unitOfWork.GetRepository<Member>()
                .GetAll( x => x.Phone == UpdateMember.Phone && x.Id != Id );
           
            if (Email.Any() || Phone.Any() == true) return false;

            var Member = unitOfWork.GetRepository<Member>().GetById( Id );
            if (Member is null) return false;

           mapper.Map(UpdateMember , Member);
           unitOfWork.GetRepository<Member>().Update( Member );
            return unitOfWork.Savechanges() > 0;
        }

        // Remove 
        public bool RemoveMember(int MemberId)
        {
           
        //    var MemberRepo = unitOfWork.GetRepository<Member>();

            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return false;

            var SessionIds = unitOfWork.GetRepository<MemberSession>()
                    .GetAll(x => x.SessionId == MemberId).Select(x => x.SessionId);

            var HasFutureSessions = unitOfWork.GetRepository<Session>().GetAll(
                x => SessionIds.Contains( x.Id) && x.StartDate > DateTime.Now).Any();
           
            if (HasFutureSessions) return false;

            var MemberShips = unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId);
           
            // Active هيمسح الاشخاص اللي هما مش
            
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var membership in MemberShips)
                    {
                        unitOfWork.GetRepository<MemberShip>().Delete( membership );
                    }
                }
                unitOfWork.GetRepository<Member>().Delete(Member) ;
               
                var IsDeleted = unitOfWork.Savechanges() > 0;
                if (IsDeleted == true)
                {
                    attachmentService.Delete(Member.Photo, "members");
                }

                return IsDeleted;
            }
            catch 
            {
                return false;
            }

        }
    }
}
