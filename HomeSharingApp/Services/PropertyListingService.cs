using HomeSharingApp.Areas.Identity.Data;
using HomeSharingApp.Models;
using HomeSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class PropertyListingService : IPropertyListingService
{
  private readonly AppDbContext dbContext;
  private readonly IWebHostEnvironment _webHostEnvironment;

    public PropertyListingService(AppDbContext _dbContext, IWebHostEnvironment webHostEnvironment)
   {
     AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
     this.dbContext = _dbContext;
     this._webHostEnvironment = webHostEnvironment;
   }
   public List<PropertyListing> SearchPropertyListing(string searchQuery)
   {
        List<PropertyListing> propertyListings = GetPropertyListings();
        if (string.IsNullOrEmpty(searchQuery)) {
            return propertyListings;
        }
        return propertyListings.Where(p => p.PropertyName.ToLower().Contains(searchQuery.ToLower()) || p.Location.ToLower().Contains(searchQuery.ToLower()) || p.Host.FirstName.ToLower().Contains(searchQuery.ToLower())).ToList();
   }
   public List<PropertyListing> GetPropertyListings()
   {
     List<PropertyListing> propertyListings = dbContext.PropertyListings.Include(u => u.Host).ToList();
     return propertyListings;
   }
 
   public PropertyListing GetPropertyListingById(int id)
   {
     PropertyListing propertyListing = dbContext.PropertyListings.FirstOrDefault(p => p.PropertyId == id);
     return propertyListing;
   }

   public void UpdatePropertyListing(PropertyListing propertyListing)
   {
       var originalPropertyListing = this.GetPropertyListingById(propertyListing.PropertyId);
       if (propertyListing.PropertyListingGallery != null)
       {
           propertyListing.PropertyListingImageUrls = new List<string>();
           foreach (var propertyImage in propertyListing.PropertyListingGallery)
           {
               string imagePath = "propertyListingImages/" + Guid.NewGuid().ToString() + "_" + propertyImage.FileName;
               string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
               propertyImage.CopyTo(new FileStream(serverPath, FileMode.Create));
               originalPropertyListing.PropertyListingImageUrls.Add("/" + imagePath);
           }
       }

       originalPropertyListing.PropertyName = propertyListing.PropertyName;
       originalPropertyListing.PropertyType = propertyListing.PropertyType;
       originalPropertyListing.Location = propertyListing.Location;
       originalPropertyListing.PricePerNight = propertyListing.PricePerNight;
       originalPropertyListing.NextAvailableDate = propertyListing.NextAvailableDate;
       originalPropertyListing.NumberOfBedrooms = propertyListing.NumberOfBedrooms;
       originalPropertyListing.NumberOfBeds = propertyListing.NumberOfBeds;
       originalPropertyListing.NumberOfBaths = propertyListing.NumberOfBaths;
       originalPropertyListing.MaxNumberOfGuests = propertyListing.MaxNumberOfGuests;
       originalPropertyListing.MinimumReservationDays = propertyListing.MinimumReservationDays;
       originalPropertyListing.Description = propertyListing.Description;
       dbContext.SaveChanges();
   }
   public void AddPropertyListing(PropertyListing propertyListing, AppUser loggedinUser)
   {
        if (propertyListing.PropertyListingGallery != null)
        {
            propertyListing.PropertyListingImageUrls = new List<string>();
            foreach (var propertyImage in propertyListing.PropertyListingGallery)
            {
                string imagePath = "propertyListingImages/" + Guid.NewGuid().ToString() + "_" + propertyImage.FileName;
                string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
                propertyImage.CopyTo(new FileStream(serverPath, FileMode.Create));
                propertyListing.PropertyListingImageUrls.Add("/" + imagePath);
            }
        }
        propertyListing.HostID = loggedinUser.Id;
        propertyListing.Host = loggedinUser;
        dbContext.PropertyListings.Add(propertyListing);
       dbContext.SaveChanges();
   }
   public void DeleteProperty(PropertyListing propertyListing)
   {
       dbContext.PropertyListings.Remove(propertyListing);
       dbContext.SaveChanges();
   }
}