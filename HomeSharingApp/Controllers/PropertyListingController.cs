using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeSharingApp.Services.Interfaces;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using HomeSharingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HomeSharingApp.Areas.Identity.Data;

namespace HomeSharing.Controllers;

[Authorize]
public class PropertyListingController : Controller
{
    private readonly IPropertyListingService _propertyListingService;
    private readonly IReviewService _reviewService;
    private readonly UserManager<AppUser> _userManager;

    public PropertyListingController(IPropertyListingService propertyListingService, IReviewService reviewService, UserManager<AppUser> userManager)
    {
        _propertyListingService = propertyListingService;
        _reviewService = reviewService;
        _userManager = userManager;
    }

    public IActionResult SearchPropertyListing(string searchQuery)
    {
        var propertyListings = this._propertyListingService.SearchPropertyListing(searchQuery);
        return View("../PropertyListing/GetPropertyListings", propertyListings);

    }

    [HttpGet]
    public IActionResult GetPropertyListings()
    {
        var propertyListings = this._propertyListingService.GetPropertyListings();
        return View(propertyListings);
    }

    public ViewResult AddPropertyListing()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPropertyListing(PropertyListing propertyListing)
    {
        AppUser loggedinUser = GetLoggedInUser();
        _propertyListingService.AddPropertyListing(propertyListing, loggedinUser);
        return RedirectToAction("GetPropertyListings");
    }

    private AppUser GetLoggedInUser()
    {
        var loggedinUserId = _userManager.GetUserId(HttpContext.User);
        AppUser loggedinUser = _userManager.FindByIdAsync(loggedinUserId).Result;
        return loggedinUser;
    }

    public IActionResult Edit(int id)
    {
        // Fetch the property listing from the database
        var propertyListing = _propertyListingService.GetPropertyListingById(id);

        // Check if the property listing exists
        if (propertyListing == null)
        {
            return NotFound();
        }

        // Check if the logged-in user is the owner of the property
        var loggedInUserId = _userManager.GetUserId(User);
        if (propertyListing.HostID != loggedInUserId)
        {
            return Forbid();
        }

        return View(propertyListing);
    }

    [HttpPost]
    public IActionResult Edit(PropertyListing propertyListing)
    {
        // Check if the logged-in user is the owner of the property
        var loggedInUserId = _userManager.GetUserId(User);
        if (propertyListing.HostID != loggedInUserId)
        {
            return Forbid();
        }

        // Update the property listing in the database
        _propertyListingService.UpdatePropertyListing(propertyListing);

        // Redirect to the property detail page
        return RedirectToAction("Detail", new { id = propertyListing.PropertyId });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        // Fetch the property listing from the database
        var propertyListing = _propertyListingService.GetPropertyListingById(id);

        // Check if the property listing exists
        if (propertyListing == null)
        {
            return NotFound();
        }

        // Check if the logged-in user is the owner of the property
        var loggedInUserId = _userManager.GetUserId(User);
        if (propertyListing.HostID != loggedInUserId)
        {
            return Forbid();
        }

        _propertyListingService.DeleteProperty(propertyListing);

        return RedirectToAction("GetPropertyListings");
    }
    public IActionResult Detail(int id)
    {
        var propertyListing = _propertyListingService.GetPropertyListingById(id);
        if (propertyListing == null)
        {
            return Redirect("/errors/404");
        }
        propertyListing.Reviews = _reviewService.GetAllReviews(propertyListing.PropertyId);
        var reservation = new Reservation();
        reservation.PropertyListing = propertyListing;
        ViewBag.AvgRating = _reviewService.calcAvgReviewRating(propertyListing.Reviews);
        return View(reservation);
    }
}