using AutoMapper;
using AwiaEgyptTravel.Application.Common.DTOs;
using AwiaEgyptTravel.Core.Entities;

namespace AwiaEgyptTravel.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tour, TourDto>().ReverseMap();
            CreateMap<TourPhoto, TourPhotoDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<EmailTemplate, EmailTemplateDto>().ReverseMap();
        }
    }
}
