using HomeSharingApp.Models;
using HomeSharingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeSharingApp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _dbContext;

        public ReviewService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddReview(Review review)
        {
            _dbContext.Reviews.Add(review);
            _dbContext.SaveChanges();
        }

        public List<Review> GetAllReviews(int propertyListingID)
        {
            List<Review> reviews = _dbContext.Reviews.Where(r => r.PropertyID == propertyListingID).Include(r => r.PropertyListing).Include(r => r.Guest).ToList();
            return reviews;
        }

        public double calcAvgReviewRating(ICollection<Review> reviews)
        {
            double totalRating = 0;
            int reviewCount = reviews.Count;

            foreach (var review in reviews)
            {
                totalRating += review.Rating;
            }

            double averageRating = reviewCount > 0 ? totalRating / reviewCount : 0;
            return averageRating;
        }
    }
}
