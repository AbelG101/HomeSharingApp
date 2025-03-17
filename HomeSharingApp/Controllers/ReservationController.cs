using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeSharingApp.Models;
using Stripe.Checkout;
using Microsoft.AspNetCore.Identity;
using HomeSharingApp.Areas.Identity.Data;
using HomeSharingApp.Services.Interfaces;

namespace HomeSharingApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPropertyListingService _propertyListingService;
        private readonly IReservationService _reservationService;
        private static Reservation _GlobalReservation;

        public ReservationController(AppDbContext context, UserManager<AppUser> userManager, IPropertyListingService propertyListingService, IReservationService reservationService)
        {
            _context = context;
            _userManager = userManager;
            _propertyListingService = propertyListingService;
            _reservationService = reservationService;
        }

        [HttpPost]
        public IActionResult CheckoutPayment(Reservation reservation)
        {
            string domainName = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var propertyListing = _propertyListingService.GetPropertyListingById(reservation.PropertyListingID);
            reservation.PropertyListing = propertyListing;
            reservation.Guest = _userManager.FindByIdAsync(reservation.GuestID).Result;
            var options = new SessionCreateOptions
            {
                SuccessUrl = domainName + $"/Reservation/Create",
                CancelUrl = domainName,
                Mode = "payment",
                CustomerEmail = reservation.Guest.Email,
                LineItems = new List<SessionLineItemOptions>(),
            };
            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long?)(100 * reservation.TotalPrice),
                    Currency = "USD",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "A trip to: " + reservation.PropertyListing.PropertyName + $" in {reservation.PropertyListing.Location}"
                    }
                },
                Quantity = 1,
            };
            options.LineItems.Add(sessionListItem);
            var service = new SessionService();
            Session session = service.Create(options);

            ReservationController._GlobalReservation = reservation;

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var loggedinUserId = _userManager.GetUserId(HttpContext.User);
            var reservations = _reservationService.GetAllReservations(loggedinUserId);
            return View(reservations);
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = _reservationService.GetReservation(id);
            ViewBag.review = new Review();

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        public async Task<IActionResult> Create()
        {
            Reservation reservation = ReservationController._GlobalReservation;

            if (ModelState.IsValid)
            {
                _reservationService.AddReservation(reservation);
                // update the next available date of the reserved property
                reservation.PropertyListing.NextAvailableDate = reservation.CheckoutDate;
                _propertyListingService.UpdatePropertyListing(reservation.PropertyListing);
                return RedirectToAction(nameof(Index));
            }
            ViewData["GuestID"] = new SelectList(_context.Users, "Id", "Id", reservation.GuestID);
            ViewData["PropertyListingID"] = new SelectList(_context.PropertyListings, "PropertyId", "Description", reservation.PropertyListingID);
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["GuestID"] = new SelectList(_context.Users, "Id", "Id", reservation.GuestID);
            ViewData["PropertyListingID"] = new SelectList(_context.PropertyListings, "PropertyId", "Description", reservation.PropertyListingID);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            var propertyListing = _propertyListingService.GetPropertyListingById(reservation.PropertyListingID);
            reservation.PropertyListing = propertyListing;
            reservation.Guest = _userManager.FindByIdAsync(reservation.GuestID).Result;

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    _reservationService.UpdateReservation(reservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GuestID"] = new SelectList(_context.Users, "Id", "Id", reservation.GuestID);
            ViewData["PropertyListingID"] = new SelectList(_context.PropertyListings, "PropertyId", "Description", reservation.PropertyListingID);
            return View(reservation);
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.ReservationID == id)).GetValueOrDefault();
        }
    }
}
