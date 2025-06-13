using AwiaEgyptTravel.Core.Enums;

namespace AwiaEgyptTravel.Core.Entities
{
    public class Tour : BaseEntity
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
        public List<TourPhoto> Photos { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class TourPhoto : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
    }

    public class Order : BaseEntity
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public string HotelName { get; set; }
        public string RoomNumber { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsEmailSent { get; set; }
        public DateTime? EmailSentDate { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
    }

    public class EmailTemplate : BaseEntity
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsDefault { get; set; }
    }
}
