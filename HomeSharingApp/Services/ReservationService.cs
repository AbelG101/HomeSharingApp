using HomeSharingApp.Models;
using HomeSharingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeSharingApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _dbContext;
        public ReservationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Reservation> GetAllReservations(string loggedinUserId)
        {
            List<Reservation> reservations = _dbContext.Reservations.Where(r => r.GuestID == loggedinUserId).Include(r => r.Guest).Include(r => r.PropertyListing).ToList();
            return reservations;
        }
        public Reservation GetReservation(int? id)
        {
            var reservation = _dbContext.Reservations
                .Include(r => r.Guest)
                .Include(r => r.PropertyListing)
                .FirstOrDefault(r => r.ReservationID == id);
            return reservation;
        }
        public void AddReservation(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }
        public void UpdateReservation(Reservation reservation)
        {
            var originalReservation = GetReservation(reservation.ReservationID);
            originalReservation.NumberOfGuests = reservation.NumberOfGuests;
            _dbContext.SaveChanges();
        }
    }
}
