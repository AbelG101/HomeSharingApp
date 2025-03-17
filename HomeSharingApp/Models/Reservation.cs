using HomeSharingApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSharingApp.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }
        public int PropertyListingID { get; set; }
        [ForeignKey("PropertyListingID")]
        public virtual PropertyListing PropertyListing { get; set; }
        public string GuestID { get; set; }
        [ForeignKey("GuestID")]
        public virtual AppUser Guest { get; set; }
        [Required]
        public DateTime CheckinDate { get; set; }
        [Required]
        public DateTime CheckoutDate { get; set; }
        [Required]
        public int NumberOfGuests { get; set; }
        [Required]
        public double TotalPrice { get; set; }
    }
}
