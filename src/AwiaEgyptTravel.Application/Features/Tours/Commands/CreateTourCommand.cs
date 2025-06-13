using AutoMapper;
using AwiaEgyptTravel.Application.Common.DTOs;
using AwiaEgyptTravel.Application.Common.Models;
using AwiaEgyptTravel.Core.Entities;
using AwiaEgyptTravel.Core.Enums;
using AwiaEgyptTravel.Core.Interfaces;
using MediatR;

namespace AwiaEgyptTravel.Application.Features.Tours.Commands
{
    public class CreateTourCommand : IRequest<BaseResponse<TourDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public string Currency { get; set; }
        public bool IsAvailable { get; set; }
        public TourCategory Category { get; set; }
        public List<string> PhotoUrls { get; set; }
    }

    public class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, BaseResponse<TourDto>>
    {
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;

        public CreateTourCommandHandler(ITourRepository tourRepository, IMapper mapper)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TourDto>> Handle(CreateTourCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tour = new Tour
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartTime = request.StartTime,
                    Duration = request.Duration,
                    Location = request.Location,
                    AdultPrice = request.AdultPrice,
                    ChildPrice = request.ChildPrice,
                    InfantPrice = request.InfantPrice,
                    Currency = request.Currency,
                    IsAvailable = request.IsAvailable,
                    Category = request.Category,
                    Photos = request.PhotoUrls.Select(url => new TourPhoto { ImageUrl = url }).ToList()
                };

                await _tourRepository.AddAsync(tour);
                var tourDto = _mapper.Map<TourDto>(tour);
                return BaseResponse<TourDto>.SuccessResponse(tourDto, "Tour created successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<TourDto>.FailureResponse(ex.Message);
            }
        }
    }
}
