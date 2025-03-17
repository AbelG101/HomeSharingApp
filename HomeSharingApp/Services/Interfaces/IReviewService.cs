using HomeSharingApp.Models;

namespace HomeSharingApp.Services.Interfaces
{
    public interface IReviewService
    {
        public List<Review> GetAllReviews(int propertyListingID);
        public void AddReview(Review review);
        public double calcAvgReviewRating(ICollection<Review> reviews);
    }
}
