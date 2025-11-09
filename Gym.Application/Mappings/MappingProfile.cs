using AutoMapper;
using Gym.Application.Commands.Membership;
using Gym.Application.DTOs;
using Gym.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Roles, opt => opt.Ignore()).ReverseMap();

            CreateMap<Memberships, MembershipDTO>().ReverseMap();

            CreateMap<CreateMembershipCommand, Memberships>()
           .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(_ => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "New"));

            CreateMap<Memberships, AttendanceLog>()
           .ForMember(dest => dest.ScanTime, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<AttendanceLog, AttendanceLogDTO>().ReverseMap();


        }
    }
}
