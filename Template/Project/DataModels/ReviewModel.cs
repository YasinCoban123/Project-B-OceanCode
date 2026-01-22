public class ReviewModel
{

    public long ReviewId { get; set; }
    public long MovieId { get; set; }
    public string Title { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }

    public ReviewModel()
    {
    }

    public ReviewModel(long reviewId, long movieId, string title, int rating, string comment)
    {
        ReviewId = reviewId;
        MovieId = movieId;
        Title = title;
        Rating = rating;
        Comment = comment;
    }
    
}