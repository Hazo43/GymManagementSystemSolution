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
        private readonly IGenericRepository<Member> MemberRepository;
        private readonly IGenericRepository<MemberShip> membershipRepository;
        private readonly IPlanRepository planRepository;
        private readonly IGenericRepository<HealthRecord> healthRecordRepository;
        private readonly IGenericRepository<MemberSession> memberSessionRepository;

        // Ask SLR For Creating Object From Services   => Services يعني لازم اروح البروجرم واعمل حجات في ال
        // CLR Will Inject Address of Object In Constractor 
        public MemberService( IGenericRepository<Member> _MemberRepository , 
                             IGenericRepository<MemberShip> _membershipRepository , 
                             IPlanRepository _planRepository , 
                             IGenericRepository<HealthRecord> _healthRecordRepository , 
                             IGenericRepository<MemberSession> _memberSessionRepository)
        {
            MemberRepository = _MemberRepository;
            membershipRepository = _membershipRepository;
            planRepository = _planRepository;
            healthRecordRepository = _healthRecordRepository;
            memberSessionRepository = _memberSessionRepository;
        }

        // Add - Create 
        public bool CreateMember(CreateMemberViewModel CreateMember)
        {
            try
            {
                // Check If Email Is Exists 
                var emailExists = MemberRepository.GetAll(x => x.Email == CreateMember.Email).Any();
                if (emailExists == true) return false;

                // Check If Phone Is Exists 
                var phoneExists = MemberRepository.GetAll(x => x.Phone == CreateMember.Phone).Any();
                if (phoneExists == true) return false;

                var member = new Member()
                {
                    Name = CreateMember.Name,
                    Email = CreateMember.Email,
                    Phone = CreateMember.Phone,
                    Gender = CreateMember.Gender,
                    DateOfBirth = CreateMember.DataOfBirth,

                    Address = new Address()
                    {
                        BuildingNumber = CreateMember.BuildingNumber,
                        City = CreateMember.City,
                        Street = CreateMember.Street,
                    },

                    HealthRecord = new HealthRecord()
                    {
                        Height = CreateMember.HealthRecordViewModel.Height,
                        Weight = CreateMember.HealthRecordViewModel.Weight,
                        BloodType = CreateMember.HealthRecordViewModel.BloodType,
                        Note = CreateMember.HealthRecordViewModel.Note,
                    },
                };

                // if freater than 0 return true
                // else return fals 
                return MemberRepository.Add(member) > 0;
            
            }
            catch 
            {
              return false;
            }

     
        }

        // Get All  => Display Data For User 
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Member = MemberRepository.GetAll();   // => IGenericRepository<TEntity> 
           
            if (Member is null || !Member.Any()) 
                  return [];
           

            var membersmodels = Member.Select(x => new MemberViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Photo = x.Photo,
                Gender = x.Gender.ToString(),
            });
            return membersmodels;
        }

        // Get Member Details
        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = MemberRepository.GetById(MemberId);

            if (member is null) return null;

            var viewModel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City} ",
            };

            // Active Membership 
            var Activemembership = membershipRepository.GetAll(x => x.MemberId == MemberId 
                                                       && x.Status == "Active").FirstOrDefault();

            if (Activemembership is not null)
            {
                viewModel.MembershipStartDate = Activemembership.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = Activemembership.EndDate.ToShortDateString();

                var plan = planRepository.GetById(Activemembership.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        // Get Member Health Record Details 
        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var memberHealthRecord = healthRecordRepository.GetById(MemberId);

            if (memberHealthRecord is null) return null;

            var viewModel = new HealthRecordViewModel()
            {
                BloodType = memberHealthRecord.BloodType,
                Height = memberHealthRecord.Height,
                Note = memberHealthRecord.Note,
                Weight = memberHealthRecord.Weight,
            };

            return viewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>

        // Update 
        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = MemberRepository.GetById(MemberId);
            if (member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
            };
        }

        public bool UpdateMember(int Id, MemberToUpdateViewModel UpdateMember)
        {
            var Email = MemberRepository.GetAll( x => x.Email == UpdateMember.Email ).Any();
            var Phone = MemberRepository.GetAll( x => x.Phone == UpdateMember.Phone ).Any();
            if (Email || Phone == true) return false;

            var Member = MemberRepository.GetById( Id );
            if (Member is null) return false;

            Member.Email = UpdateMember.Email;
            Member.Phone = UpdateMember.Phone;
            Member.Address.BuildingNumber = UpdateMember.BuildingNumber;
            Member.Address.City = UpdateMember.City;
            Member.Address.Street = UpdateMember.Street;

            ///// => false لو محدش عدل هيرجع true لو حد عدل هيرجع

            return MemberRepository.Update( Member ) > 0;
        }

        // Remove 
        public bool RemoveMember(int MemberId)
        {
           

            var Member = MemberRepository.GetById(MemberId);
            if (Member is null) return false;

            var HasActiveMemberSession = memberSessionRepository 
                    .GetAll( x => x.SessionId == MemberId && x.Session.StartDate > DateTime.Now ).Any();
          
            if (HasActiveMemberSession) return false;

            var MemberShips = memberSessionRepository.GetAll(X => X.MemberId == MemberId);
           
            // Active هيمسح الاشخاص اللي هما مش
            
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var membership in MemberShips)
                    {
                        memberSessionRepository.Delete( membership );
                    }
                }
               return MemberRepository.Delete(Member) > 0;
            }
            catch 
            {
                return false;
            }

        }
    }
}
