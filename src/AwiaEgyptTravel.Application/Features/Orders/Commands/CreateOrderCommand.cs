using AutoMapper;
using AwiaEgyptTravel.Application.Common.DTOs;
using AwiaEgyptTravel.Application.Common.Models;
using AwiaEgyptTravel.Core.Entities;
using AwiaEgyptTravel.Core.Interfaces;
using MediatR;

namespace AwiaEgyptTravel.Application.Features.Orders.Commands
{
    public class CreateOrderCommand : IRequest<BaseResponse<OrderDto>>
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string HotelName { get; set; }
        public string RoomNumber { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }        public int TourId { get; set; }
        public string TransactionId { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, BaseResponse<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            ITourRepository tourRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _tourRepository = tourRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tour = await _tourRepository.GetByIdAsync(request.TourId);
                if (tour == null)
                {
                    return BaseResponse<OrderDto>.FailureResponse("Tour not found");
                }

                decimal totalAmount = CalculateTotalAmount(tour, request.AdultCount, request.ChildCount, request.InfantCount);

                var order = new Order
                {
                    CustomerName = request.CustomerName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City,
                    Country = request.Country,
                    ZipCode = request.ZipCode,
                    HotelName = request.HotelName,
                    RoomNumber = request.RoomNumber,
                    AdultCount = request.AdultCount,
                    ChildCount = request.ChildCount,
                    InfantCount = request.InfantCount,
                    TotalAmount = totalAmount,
                    TourId = request.TourId,
                    TransactionId = request.TransactionId,
                    IsEmailSent = false
                };

                await _orderRepository.AddAsync(order);

                var template = await _emailTemplateRepository.GetDefaultTemplateAsync();
                if (template != null)
                {
                    await _emailService.SendOrderConfirmationEmailAsync(order, template);
                }

                var orderDto = _mapper.Map<OrderDto>(order);
                return BaseResponse<OrderDto>.SuccessResponse(orderDto, "Order created successfully");
            }
            catch (Exception ex)
            {
                return BaseResponse<OrderDto>.FailureResponse(ex.Message);
            }
        }

        private decimal CalculateTotalAmount(Tour tour, int adultCount, int childCount, int infantCount)
        {
            return (tour.AdultPrice * adultCount) +
                   (tour.ChildPrice * childCount) +
                   (tour.InfantPrice * infantCount);
        }
    }
}
