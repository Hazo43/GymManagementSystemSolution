﻿using AutoMapper;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
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
            // Display 
            CreateMap<Session, SessionViewModel>()
                     .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(Src => Src.SessionCategory.CategoryName))
                     .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(Src => Src.SessionTrainer.Name))
                     .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            //Create
            CreateMap<CreateSessionViewModel, Session>();

           // Update 
            CreateMap< Session , UpdateSessionViewModel>().ReverseMap();

            // ReverseMapدا المقصود ب ال 
            // Destination و ال Source هتعمل واحده نفس للي فوق بس هيعكس ال
            
            // CreateMap< UpdateSessionViewModel , Session>();

        }
    }
}
