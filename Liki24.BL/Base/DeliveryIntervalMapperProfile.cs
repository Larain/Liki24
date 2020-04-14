using System;
using System.Linq;
using AutoMapper;
using Liki24.Contracts.Models;
using Liki24.DAL.Models;
using DeliveryIntervalType = Liki24.Contracts.Models.DeliveryIntervalType;

namespace Liki24.BL.Base
{
    public class DeliveryIntervalMapperProfile : Profile
    {
        private const string TimeFormat = @"hh\:mm";
        public DeliveryIntervalMapperProfile()
        {
            CreateMap<DeliveryIntervalBase, DeliveryInterval>()
                .ForMember(dest => dest.AvailableFrom, opt => opt.MapFrom(src => TimeSpan.Parse(src.AvailableFrom)))
                .ForMember(dest => dest.AvailableTo, opt => opt.MapFrom(src => TimeSpan.Parse(src.AvailableTo)))
                .ForMember(dest => dest.ExpectedFrom, opt => opt.MapFrom(src => TimeSpan.Parse(src.ExpectedFrom)))
                .ForMember(dest => dest.ExpectedTo, opt => opt.MapFrom(src => TimeSpan.Parse(src.ExpectedTo)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (DeliveryIntervalType)src.Type.Id))
                .IncludeAllDerived();

            CreateMap<DeliveryInterval, DeliveryIntervalBase>()
                .ForMember(dest => dest.AvailableFrom, opt => opt.MapFrom(src => src.AvailableFrom.ToString(TimeFormat)))
                .ForMember(dest => dest.AvailableTo, opt => opt.MapFrom(src => src.AvailableTo.ToString(TimeFormat)))
                .ForMember(dest => dest.ExpectedFrom, opt => opt.MapFrom(src => src.ExpectedFrom.ToString(TimeFormat)))
                .ForMember(dest => dest.ExpectedTo, opt => opt.MapFrom(src => src.ExpectedTo.ToString(TimeFormat)))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => new Value((int)src.Type, src.Type.ToString())))
                .IncludeAllDerived();

            CreateMap<DeliveryInterval, DeliveryIntervalDto>()
                .ForMember(dest => dest.AvailableDaysOfWeek,
                    opt => opt.MapFrom(src => src.AvailableDaysOfWeek.Select(x => new Value((int) x, x.ToString()))));
            CreateMap<DeliveryIntervalDto, DeliveryInterval>()
                .ForMember(dest => dest.AvailableDaysOfWeek,
                    opt => opt.MapFrom(src => src.AvailableDaysOfWeek.Select(x => (DayOfWeek) x.Id)));
            CreateMap<DeliveryInterval, ClientDeliveryInterval>();
            CreateMap<ClientDeliveryInterval, DeliveryInterval>();
        }
    }
}