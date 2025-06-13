using AwiaEgyptTravel.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AwiaEgyptTravel.Web.Models
{
    public class TourListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal AdultPrice { get; set; }
        public string Currency { get; set; }
        public string MainPhotoUrl { get; set; }
        public TourCategory Category { get; set; }
    }

    public class TourDetailViewModel
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
        public List<string> PhotoUrls { get; set; }
    }

    public class CreateTourViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal AdultPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ChildPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal InfantPrice { get; set; }

        [Required]
        public string Currency { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public TourCategory Category { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}
