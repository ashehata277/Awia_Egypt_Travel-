using System.ComponentModel.DataAnnotations;

namespace AwiaEgyptTravel.Web.Models
{
    public class CartItemViewModel
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public string Currency { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [Display(Name = "ZIP Code")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [CreditCard]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Hotel Name")]
        public string HotelName { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        public List<CartItemViewModel> CartItems { get; set; }

        public decimal TotalAmount => CartItems?.Sum(x => x.TotalAmount) ?? 0;
    }
}
