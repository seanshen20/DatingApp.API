using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            //ForMember(dest => dest.Value, m => m.MapFrom(src => src.Value + 10)));
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, m => m.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => {opt.MapFrom(d => d.DateOfBirth.CaculateAge());});
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, m => m.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => { opt.MapFrom(d => d.DateOfBirth.CaculateAge()); });

            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
