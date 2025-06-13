using System.ComponentModel.DataAnnotations;

namespace AwiaEgyptTravel.Web.Models
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string TourName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsEmailSent { get; set; }
        public DateTime? EmailSentDate { get; set; }
    }

    public class OrderDetailViewModel
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
        public TourDetailViewModel Tour { get; set; }
    }

    public class EmailTemplateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public bool IsDefault { get; set; }

        public string AvailablePlaceholders => "{CustomerName}, {TourName}, {TotalAmount}, {OrderDate}";
    }
}
