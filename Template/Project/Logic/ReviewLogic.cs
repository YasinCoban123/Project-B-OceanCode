using System.Collections.Generic;

public class ReviewLogic
{
	private readonly ReviewAcces _access = new ReviewAcces();

	public List<ReviewModel> GetReviewsForMovie(long movieId)
	{
		return _access.GetReviewsForMovie(movieId);
	}

	public long AddReview(ReviewModel review)
	{
		return _access.AddReview(review);
	}
}