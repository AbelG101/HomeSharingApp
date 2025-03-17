using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HomeSharingApp.Models;
using Microsoft.AspNetCore.Identity;
// using Microsoft.Build.Framework;

namespace HomeSharingApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    [PersonalData]
    [Required]
    public string FirstName { get; set; }

    [PersonalData]
    [Required]
    public string LastName { get; set; }

    public string? ProfilePictureUrl { get; set; }
    [NotMapped]
    [Display(Name = "Profile Picture")]
    public IFormFile? ProfilePic { get; set; }
    public virtual ICollection<PropertyListing> PropertyListings { get; set; }
    public virtual ICollection<Reservation>? Reservations { get; set; }

}

