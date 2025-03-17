using HomeSharingApp.Areas.Identity.Data;
using HomeSharingApp.Models; 

namespace HomeSharingApp.Services.Interfaces;

public interface IPropertyListingService
{
  public List<PropertyListing> GetPropertyListings();
  public List<PropertyListing> SearchPropertyListing(string searchQuery);
  public void AddPropertyListing(PropertyListing propertyListing, AppUser loggedinUser);
  public void UpdatePropertyListing(PropertyListing propertyListing);
  public PropertyListing GetPropertyListingById(int id);
  public void DeleteProperty(PropertyListing propertyListing);
}