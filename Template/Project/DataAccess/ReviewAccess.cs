using Microsoft.Data.Sqlite;
using Dapper;
public class ReviewAcces
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private const string Table = "Review";

    public List<ReviewModel> GetReviewsForMovie(long movieId)
    {
        string sql = $"SELECT * FROM {Table} WHERE MovieId = @MovieId";
        return _connection.Query<ReviewModel>(sql, new { MovieId = movieId }).ToList();
    }
    
}