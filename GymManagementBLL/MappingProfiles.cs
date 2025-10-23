using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewMpdels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {        
         
            MapSession();

            MapMember();

            MapTrainer();

            MapPlan();
        }
        private void MapMember()
        {
            // Create 
            CreateMap<CreateMemberViewModel, Member>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                 .ForMember( dest => dest.HealthRecord , opt => opt.MapFrom( src => src.HealthRecordViewModel));

            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));

            // HealthRecord
            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            // GetAll and GetById 
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));


            // Update => Display 
            CreateMap<Member, MemberToUpdateViewModel>()
                  .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                  .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                  .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));



            // Member Update 
            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.UtcNow;
                });
               
                
        }

        private void MapSession ()
        {
            // Display 
            CreateMap<Session, SessionViewModel>()
                     .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(Src => Src.SessionCategory.CategoryName))
                     .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(Src => Src.SessionTrainer.Name))
                     .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            //
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, option => option.MapFrom(src => src.CategoryName));

            //Create
            CreateMap<CreateSessionViewModel, Session>();

            // Update 
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            // ReverseMapدا المقصود ب ال 
            // Destination و ال Source هتعمل واحده نفس للي فوق بس هيعكس ال

            // CreateMap< UpdateSessionViewModel , Session>();
        }

        private void MapTrainer()
        {
            // Create 
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                }));


            // GetAll && GetById
            CreateMap<Trainer, TrainerViewModel>();

            // Update => Display 
            CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            // UpdateTrainer 
            CreateMap<UpdateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.UpdatedAt = DateTime.UtcNow;
                });
        }
        private void MapPlan()
        {
            // GetAll & GetById 
            CreateMap<Plan, PlanViewModel>();

            // Update => Display 
            CreateMap<Plan , UpdatePlanViewModel>()
                .ForMember( dest => dest.PlanName , opt => opt.MapFrom( src => src.Name));

            // Update Plan 
            CreateMap<UpdatePlanViewModel , Plan>()
                .ForMember( dest => dest.Name , opt => opt.Ignore())
                .ForMember( dest => dest.UpdatedAt , opt => opt.MapFrom( src => DateTime.Now));

        }
    }
}
