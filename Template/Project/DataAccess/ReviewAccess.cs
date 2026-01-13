using Microsoft.Data.Sqlite;
using Dapper;
public class ReviewAcces
{
    private const string Table = "Review";
    private string _dbPath = "DataSources/project.db";

    private void EnsureTableExists(SqliteConnection conn)
    {
        string ddl = 
            $@"CREATE TABLE IF NOT EXISTS {Table} (
                ReviewId INTEGER PRIMARY KEY AUTOINCREMENT,
                MovieId INTEGER NOT NULL,
                Title TEXT NOT NULL,
                Rating INTEGER NOT NULL,
                Comment TEXT,
                FOREIGN KEY (MovieId) REFERENCES Movie(MovieId)
            );";
        using var cmd = conn.CreateCommand();
        cmd.CommandText = ddl;
        cmd.ExecuteNonQuery();
    }

    public List<ReviewModel> GetReviewsForMovie(long movieId)
    {
        string sql = $"SELECT * FROM {Table} WHERE MovieId = @MovieId";
        using var conn = new SqliteConnection($"Data Source={_dbPath}");
        conn.Open();
        EnsureTableExists(conn);
        return conn.Query<ReviewModel>(sql, new { MovieId = movieId }).ToList();
    }

    public long AddReview(ReviewModel review)
    {
        string sql = $"INSERT INTO {Table} (MovieId, Title, Rating, Comment) VALUES (@MovieId, @Title, @Rating, @Comment); SELECT last_insert_rowid();";
        using var conn = new SqliteConnection($"Data Source={_dbPath}");
        conn.Open();
        EnsureTableExists(conn);
        return conn.ExecuteScalar<long>(sql, new { review.MovieId, review.Title, review.Rating, review.Comment });
    }
}