using Microsoft.Data.Sqlite;
using Dapper;
public class ReviewAcces : DataAccess
{
    protected override string Table => "Review";

    public List<ReviewModel> GetReviewsForMovie(long movieId)
    {
        string sql = $"SELECT * FROM {Table} WHERE MovieId = @MovieId";
        return _connection.Query<ReviewModel>(sql, new { MovieId = movieId }).ToList();
    }

    public long AddReview(ReviewModel review)
    {
        string sql = $"INSERT INTO {Table} (MovieId, Title, Rating, Comment) VALUES (@MovieId, @Title, @Rating, @Comment);";
        return _connection.Execute(sql, new { review.MovieId, review.Title, review.Rating, review.Comment });
    }
}