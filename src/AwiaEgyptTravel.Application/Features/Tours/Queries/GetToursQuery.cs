using AutoMapper;
using AwiaEgyptTravel.Application.Common.DTOs;
using AwiaEgyptTravel.Application.Common.Models;
using AwiaEgyptTravel.Core.Entities;
using AwiaEgyptTravel.Core.Interfaces;
using MediatR;

namespace AwiaEgyptTravel.Application.Features.Tours.Queries
{
    public class GetToursQuery : IRequest<BaseResponse<List<TourDto>>>
    {
        public bool OnlyAvailable { get; set; }
    }

    public class GetToursQueryHandler : IRequestHandler<GetToursQuery, BaseResponse<List<TourDto>>>
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public GetToursQueryHandler(ITourRepository tourRepository, IMapper mapper)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<TourDto>>> Handle(GetToursQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tours = request.OnlyAvailable 
                    ? await _tourRepository.GetAvailableToursAsync()
                    : await _tourRepository.ListAllAsync();

                var tourDtos = _mapper.Map<List<TourDto>>(tours);
                return BaseResponse<List<TourDto>>.SuccessResponse(tourDtos);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<TourDto>>.FailureResponse(ex.Message);
            }
        }
    }
}
