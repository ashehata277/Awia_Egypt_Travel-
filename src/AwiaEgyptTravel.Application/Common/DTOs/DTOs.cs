using AwiaEgyptTravel.Core.Enums;

namespace AwiaEgyptTravel.Application.Common.DTOs
{
    public class TourDto
    {
        public int Id { get; set; }
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
        public List<TourPhotoDto> Photos { get; set; }
    }

    public class TourPhotoDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
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
        public int InfantCount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsEmailSent { get; set; }
        public DateTime? EmailSentDate { get; set; }
        public TourDto Tour { get; set; }
    }

    public class EmailTemplateDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsDefault { get; set; }
    }
}
