using HomeSharingApp.Models;
using Stripe;

namespace HomeSharingApp.Services.Interfaces
{
    public interface IReservationService
    {
        public List<Reservation> GetAllReservations(string loggedinUserId);
        public Reservation GetReservation(int? id);
        public void AddReservation(Reservation reservation);
        public void UpdateReservation(Reservation reservation);
    }
}
