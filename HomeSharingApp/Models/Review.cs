using HomeSharingApp.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeSharingApp.Models
{
    public class Review
    {   
        public Review()
        {
            ReviewDate = DateTime.Now;
        }

        [Key]
        public int ReviewID { get; set; }
        public string GuestID { get; set; }
        [ForeignKey("GuestID")]
        public virtual AppUser Guest { get; set; }
        public int PropertyID { get; set; }
        [ForeignKey("PropertyID")]
        public virtual PropertyListing PropertyListing { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        [Required]
        public string ReviewContent { get; set; }
    }
}
