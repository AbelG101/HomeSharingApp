using HomeSharingApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSharingApp.Models
{
    public class PropertyListing
    {
        [Key]
        public int PropertyId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string HostID { get; set; }

        [ForeignKey(nameof(HostID))]
        public virtual AppUser Host { get; set; }

        [Required]
        [MaxLength(100)]
        public string? PropertyName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PropertyType { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Location { get; set; }

        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal PricePerNight { get; set; }

        [Required]
        public DateTime NextAvailableDate { get; set; }

        [Required]
        public int NumberOfBedrooms { get; set; }

        [Required]
        public int NumberOfBeds { get; set; }

        [Required]
        public int NumberOfBaths { get; set; }

        [Required]
        public int MaxNumberOfGuests { get; set; }

        [Required]
        public int MinimumReservationDays { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "text[]")]
        public List<string>? PropertyListingImageUrls { get; set; }
        [NotMapped]
        [Display(Name = "Image of property listing")]
        public List<IFormFile>? PropertyListingGallery { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
