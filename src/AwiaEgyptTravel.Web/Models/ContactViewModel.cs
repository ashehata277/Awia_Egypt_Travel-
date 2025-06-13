using System.ComponentModel.DataAnnotations;

namespace AwiaEgyptTravel.Web.Models
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }
    }
}
