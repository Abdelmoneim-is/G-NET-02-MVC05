using AutoMapper;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModel;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {



            MapMember();
            SessionMapper();






        }

        private void MapMember()
        {
            CreateMap<Member, MemberViewModels>()
                .ForMember(dest => dest.Address, map => map.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));
            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
            CreateMap<Member, MemberViewModelEdit>()
                .ForMember(dest => dest.BuildingNumber, map => map.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, map => map.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, map => map.MapFrom(src => src.Address.City));

            CreateMap<MemberViewModelEdit, Member>()
                .ForMember(dest => dest.Name, map => map.Ignore())
                .ForMember(dest => dest.Photo, map => map.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, map => map.MapFrom(src => new Address()
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                }))
                .ForMember(dest => dest.healthRecord, map => map.MapFrom(src => src.HealthRecordViewModel));
        }

        private void SessionMapper()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, GetTrainerBySelectViewModel>();
            CreateMap<Category, GetCategoryBySelectViewModel>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category.CategoryName));

            CreateMap<Session , UpdateSessionViewModel>().ReverseMap();
        }
    }
}
