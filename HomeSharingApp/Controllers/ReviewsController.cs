using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeSharingApp.Models;
using HomeSharingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using HomeSharingApp.Services.Interfaces;

namespace HomeSharingApp.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPropertyListingService _propertyListingService;
        private readonly IReviewService _reviewService;

        public ReviewsController(AppDbContext context, UserManager<AppUser> userManager, IPropertyListingService propertyListingService, IReviewService reviewService)
        {
            // _context = context;
            _userManager = userManager;
            _propertyListingService = propertyListingService;
            _reviewService = reviewService;
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            var propertyListing = _propertyListingService.GetPropertyListingById(review.PropertyID);
            review.PropertyListing = propertyListing;
            review.Guest = _userManager.FindByIdAsync(review.GuestID).Result;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                _reviewService.AddReview(review);
            }

            return Redirect($"/PropertyListing/Detail/{review.PropertyID}");
        }
    }
}
